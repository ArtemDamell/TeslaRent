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

		public async Task<TeslaCarDTO> CreateCar(TeslaCarDTO carForCreation)
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
			var addedCar = await _db.TeslaCars.AddAsync(car);

			// 24.4 Сохраняем на основе добавленых сущьностей изменения в базе данных
			await _db.SaveChangesAsync();

			return _mapper.Map<TeslaCar, TeslaCarDTO>(addedCar.Entity);
			// --> Далее, реализовываем остальные методы
		}

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

		public async Task<IEnumerable<TeslaCarDTO>> GetAllCars()
		{
			/* 
			 * Т.к. логика может вызвать разнообразные ошибки и обвалить
			 * Приложение, далее будем по возможности использовать конструкцию
			 * try/catch
			 */
			try
			{
				IEnumerable<TeslaCarDTO> teslaCarDTOs =
					_mapper.Map<IEnumerable<TeslaCar>, IEnumerable<TeslaCarDTO>>(await _db.TeslaCars.Include(x => x.CarAccessories).ToListAsync());

				return teslaCarDTOs;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<TeslaCarDTO> GetCar(int carId)
		{
			try
			{
				TeslaCarDTO car = _mapper.Map<TeslaCar, TeslaCarDTO>(
					await _db.TeslaCars.FindAsync(carId));

				return car;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		// 49.2 Добавляем параметр int carId = 0
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
				// ***************************************************************

				//TeslaCarDTO car = _mapper.Map<TeslaCar, TeslaCarDTO>(
				//    await _db.TeslaCars.FirstOrDefaultAsync(x => x.Name.ToLower() == carName.ToLower()));

				//return car;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<TeslaCarDTO> UpdateCar(int carId, TeslaCarDTO carForUpdating)
		{
			try
			{
				if (carId == carForUpdating.Id)
				{
					// Получаем данные из базы
					var carDetailsFromDb = await _db.TeslaCars.FindAsync(carId);

                    foreach (var item in carForUpdating.CarAccessories)
                    {
						item.CarId = carId;
                    }

					// Конвертируем полученные данные из DTO в обычную модель для сохранения в базе
					var car = _mapper.Map<TeslaCarDTO, TeslaCar>(carForUpdating, carDetailsFromDb);

					// Добавляем недостающие свойства
					car.UpdatedBy = "";
					car.UpdatedDate = DateTime.UtcNow;

					// Обновляем данные в сущностях Entity
					var updatedCar = _db.Update(car);

					// Сохраняем изменения в базе данных
					await _db.SaveChangesAsync();

					return _mapper.Map<TeslaCar, TeslaCarDTO>(updatedCar.Entity);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}
	}
}
