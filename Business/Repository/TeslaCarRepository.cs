using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Diagnostics;

namespace Business.Repository
{
    // 19. Создаём класс реализации интерфейса
    public class TeslaCarRepository : ITeslaCarRepository
    {
        // 19.1 Внедряем зависимость базы данных через конструктор класса
        private readonly ApplicationDbContext _db;

        // 24. Внедряем зависимость AutoMapper
        private readonly IMapper _mapper;

        public TeslaCarRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        /// <summary>
        /// Creates a new Tesla car using the provided TeslaCarDTO object.
        /// </summary>
        /// <param name="carForCreation">TeslaCarDTO object containing the car information.</param>
        /// <returns>TeslaCarDTO object containing the created car information.</returns>
        public async Task<TeslaCarDTO> CreateCar(TeslaCarDTO carForCreation)
        {
            try
            {
                // 24.1 Конвертируем с помощью AutoMapper модель DTO в обычную
                TeslaCar car = _mapper.Map<TeslaCarDTO, TeslaCar>(carForCreation);

                // 24.2 Добавляем в модель недостающие данные
                car.CreatedDate = DateTime.UtcNow;
                car.CreatedBy = "";


                // 19.2
                /*
				 * Добавляем реализацию метода, которая приведёт к ошибки
				 * т.к. мы пытаемся сохранить через модель объект DTO
				 * Для того, чтобы исправить эту проблему, мы будем использовать
				 * библиотеку AutoMapper
				*/
                //var addedCar = _db.TeslaCars.Add(carForCreation);
                // --> На этом этапе идём реализовывать функционал AutoMapper Profile

                // 24.3 Добавляем объект в сущности Entity
                var addedCar = _db.TeslaCars.Add(car);

                // 24.4 Сохраняем на основе добавленых сущьностей изменения в базе данных
                await _db.SaveChangesAsync();

                return _mapper.Map<TeslaCar, TeslaCarDTO>(addedCar.Entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            // --> Далее, реализовываем остальные методы
        }

        /// <summary>
        /// Deletes a car from the database based on the carId.
        /// </summary>
        /// <param name="carId">The id of the car to be deleted.</param>
        /// <returns>The number of changes made to the database.</returns>
        public async Task<int> DeleteCar(int carId)
        {
            var carDetails = await _db.TeslaCars.FindAsync(carId);

            if (carDetails is not null)
            {
                // 78. Получаем все картинки для удаления и удаляем их
                var allImages = await _db.TeslaCarImages.Where(x => x.CarId == carId).ToListAsync();
                _db.TeslaCarImages.RemoveRange(allImages);

                _db.TeslaCars.Remove(carDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        // 167.1 Обновить методы в TeslaCarRepository
        /// <summary>
        /// Gets all cars from the database and checks if they are booked in the specified period.
        /// </summary>
        /// <param name="startRentDate">The start date of the rental period.</param>
        /// <param name="endRentDate">The end date of the rental period.</param>
        /// <returns>A list of TeslaCarDTO objects.</returns>
        public async Task<IEnumerable<TeslaCarDTO>> GetAllCars(string? startRentDate = null, string? endRentDate = null)
        {
            /* 
			 * Т.к. логика может вызвать разнообразные ошибки и обвалить
			 * Приложение, далее будем по возможности использовать конструкцию
			 * try/catch
			 */
            try
            {
                IEnumerable<TeslaCarDTO> teslaCarDTOs =
                    _mapper.Map<IEnumerable<TeslaCar>, IEnumerable<TeslaCarDTO>>(await _db.TeslaCars.Include(x => x.TeslaCarImages).ToListAsync());

                // 219.3 Далее копируем логику в метод GetAllCars
                if (!string.IsNullOrWhiteSpace(startRentDate) && !string.IsNullOrWhiteSpace(endRentDate))
                {
                    foreach (var car in teslaCarDTOs)
                    {
                        car.IsBooked = await IsCarBookedAsync(car.Id, startRentDate, endRentDate);
                    }
                }

                return teslaCarDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // 167.2 Обновить методы в TeslaCarRepository
        /// <summary>
        /// Retrieves a Tesla car from the database with the specified carId, and checks if it is booked between the specified start and end dates.
        /// </summary>
        /// <param name="carId">The ID of the car to retrieve.</param>
        /// <param name="startRentDate">The start date of the rental period.</param>
        /// <param name="endRentDate">The end date of the rental period.</param>
        /// <returns>A TeslaCarDTO object representing the retrieved car.</returns>
        public async Task<TeslaCarDTO> GetCar(int carId, string? startRentDate = null, string? endRentDate = null)
        {
            try
            {
                // Получаем машину из базы с подключённой таблицей
                var car = await _db.TeslaCars.Include(x => x.TeslaCarImages).FirstOrDefaultAsync(x => x.Id == carId);
                // Перегоняем в DTO
                var carDTO = _mapper.Map<TeslaCar, TeslaCarDTO>(car);

                // 219.2 Далее в TeslaCarRepository в методе GetCar
                // Далее копируем логику в метод GetAllCars
                if (!string.IsNullOrWhiteSpace(startRentDate) && !string.IsNullOrWhiteSpace(endRentDate))
                    carDTO.IsBooked = await IsCarBookedAsync(carId, startRentDate, endRentDate);

                return carDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // 49.2 Добавляем параметр int carId = 0
        /// <summary>
        /// Checks if the car is unique by name and id.
        /// </summary>
        /// <param name="carName">Name of the car.</param>
        /// <param name="carId">Id of the car.</param>
        /// <returns>TeslaCarDTO object.</returns>
        public async Task<TeslaCarDTO> IsCarUnique(string carName, int carId = 0)
        {
            try
            {
                // 49.3 Проверяем, если Id равен 0, то мы создаём а не редактируем
                if (carId.Equals(0))
                {
                    TeslaCarDTO car = _mapper.Map<TeslaCar, TeslaCarDTO>(
                    await _db.TeslaCars.FirstOrDefaultAsync(x => x.Name.ToLower() == carName.ToLower()));

                    return car;
                }
                else
                {
                    TeslaCarDTO car = _mapper.Map<TeslaCar, TeslaCarDTO>(
                    await _db.TeslaCars.FirstOrDefaultAsync(x => x.Name.ToLower() == carName.ToLower()
                    && !x.Id.Equals(carId)));

                    return car;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Updates a Tesla car in the database.
        /// </summary>
        /// <param name="carId">The ID of the car to update.</param>
        /// <param name="carForUpdating">The car data to update.</param>
        /// <returns>The updated car data.</returns>
        public async Task<TeslaCarDTO> UpdateCar(int carId, TeslaCarDTO carForUpdating)
        {
            try
            {
                if (carId == carForUpdating.Id)
                {
                    // Получаем данные из базы
                    var carDetailsFromDb = await _db.TeslaCars.Include(x => x.TeslaCarImages).FirstOrDefaultAsync(x => x.Id == carId);

                    // Конвертируем полученные данные из DTO в обычную модель для сохранения в базе
                    var car = _mapper.Map<TeslaCarDTO, TeslaCar>(carForUpdating, carDetailsFromDb);

                    // Добавляем недостающие свойства
                    car.UpdatedBy = "";
                    car.UpdatedDate = DateTime.UtcNow;

                    // Обновляем данные в сущностях Entity
                    var updatedCar = _db.Update(car);

                    // Сохраняем изменения в базе данных
                    await _db.SaveChangesAsync();

                    var result = _mapper.Map<TeslaCar, TeslaCarDTO>(car);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets all car accessories from the database.
        /// </summary>
        /// <returns>
        /// An enumerable of CarAccessoryDTO objects.
        /// </returns>
        public async Task<IEnumerable<CarAccessoryDTO>> GetAllCarAccessories()
        {
            var allAccessories = _mapper.Map<IEnumerable<CarAccessory>, IEnumerable<CarAccessoryDTO>>(await _db.CarAccessories.AsNoTracking().ToListAsync());
            return allAccessories;
        }

        /// <summary>
        /// Gets a single car accessory from the database by its Id.
        /// </summary>
        /// <param name="id">The Id of the car accessory.</param>
        /// <returns>A CarAccessoryDTO object.</returns>
        public async Task<CarAccessoryDTO> GetSingleAccessory(int id)
        {
            var accessory = _mapper.Map<CarAccessory, CarAccessoryDTO>(await _db.CarAccessories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id));
            return accessory;
        }

        // 217.2 Перенести метод IsCarBookedAsync из CarOrderDetailsRepository в TeslaCarRepository
        // 218. Изменить метод IsCarBookedAsync
        /// <summary>
        /// Checks if a car is booked for a given date range
        /// </summary>
        /// <param name="carId">Id of the car</param>
        /// <param name="startRentDateString">Start date of the rental period</param>
        /// <param name="endRentDateString">End date of the rental period</param>
        /// <returns>True if the car is booked, false otherwise</returns>
        public async Task<bool> IsCarBookedAsync(int carId, string startRentDateString, string endRentDateString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(startRentDateString) && !string.IsNullOrWhiteSpace(endRentDateString))
                {
                    var startRentDate = DateTime.ParseExact(startRentDateString, "MM.dd.yyyy", null);
                    var endRentDate = DateTime.ParseExact(endRentDateString, "MM.dd.yyyy", null);

                    var existingBooking = await _db.CarOrderDetails.Where(x =>
                                                                                     x.CarId == carId &&
                                                                                     x.IsPaymentSuccessful &&
                                                                                     ((startRentDate < x.EndRentDate && endRentDate.Date >= x.StartRentDate) ||
                                                                                     (endRentDate.Date > x.StartRentDate.Date && startRentDate.Date <= x.StartRentDate.Date)))
                                                                                     .FirstOrDefaultAsync();

                    if (existingBooking is not null)
                        return true;

                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
