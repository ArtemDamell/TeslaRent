using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models.DTO;

namespace Business.Repository
{
    // 183. Имплементируем класс реализации нового интерфейса
    public class CarOrderDetailsRepository : ICarOrderDetailsRepository
    {
        // --> 183.1 После имплементации методов, добавляем карту для автомаппера в MappingProfile 

        // 183.2 Внедряем зависимости базы и маппера
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CarOrderDetailsRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new car order details object and saves it to the database.
        /// </summary>
        /// <param name="carDetails">The car order details object to be created.</param>
        /// <returns>The created car order details object.</returns>
        public async Task<CarOrderDetailsDTO> CreateAsync(CarOrderDetailsDTO carDetails)
        {
            try
            {
                carDetails.StartRentDate = carDetails.StartRentDate.Date;
                carDetails.EndRentDate = carDetails.EndRentDate.Date;

                var carOrderDetails = _mapper.Map<CarOrderDetailsDTO, CarOrderDetails>(carDetails);
                carOrderDetails.Status = Status.Pending;

                var result = await _db.CarOrderDetails.AddAsync(carOrderDetails);
                await _db.SaveChangesAsync();

                var dto = _mapper.Map<CarOrderDetails, CarOrderDetailsDTO>(result.Entity);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Car order creation failed");
            }
        }

        /// <summary>
        /// Retrieves all car order details from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of CarOrderDetailsDTO objects.
        /// </returns>
        public async Task<IEnumerable<CarOrderDetailsDTO>> GetAllCarOrderDetailsAsync()
        {
            try
            {
                var carOrders = _mapper.Map<IEnumerable<CarOrderDetails>, IEnumerable<CarOrderDetailsDTO>>
                    (_db.CarOrderDetails.Include(x => x.TeslaCar));

                return carOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("Getting car order failed");
            }
        }

        /// <summary>
        /// Retrieves the car order details for the given car order id.
        /// </summary>
        /// <param name="carOrderId">The car order id.</param>
        /// <returns>
        /// The car order details for the given car order id.
        /// </returns>
        public async Task<CarOrderDetailsDTO> GetCarOrderDetailsAsync(int carOrderId)
        {
            try
            {
                var carOrder = await _db.CarOrderDetails
                                                                            .Include(x => x.TeslaCar)
                                                                            .ThenInclude(x => x.TeslaCarImages)
                                                                            .FirstOrDefaultAsync(x => x.Id.Equals(carOrderId));
                var carOrderDTO = _mapper.Map<CarOrderDetails, CarOrderDetailsDTO>(carOrder);
                carOrderDTO.TeslaCarDTO.TotalDays = carOrderDTO.EndRentDate.Subtract(carOrderDTO.StartRentDate).Days;

                return carOrderDTO;
            }
            catch (Exception ex)
            {

                throw new Exception("Getting car order details by ID failed");
            }
        }

        // 217.1 Перенести метод IsCarBookedAsync из CarOrderDetailsRepository в TeslaCarRepository
        // Эти 2 метода реализуем позже, не на шаге 183!
        /// <summary>
        /// Marks the payment of a car order as successful.
        /// </summary>
        /// <param name="id">The id of the car order.</param>
        /// <returns>The updated car order details.</returns>
        public async Task<CarOrderDetailsDTO> MarkPaymentSuccessfulAsync(int id)
        {
            // 215. Реализовать метод MarkPaymentSuccessfulAsync в CarOrderDetailsRepository
            var data = await _db.CarOrderDetails.FindAsync(id);
            if (data is null)
                return null;
            if (!data.IsPaymentSuccessful)
            {
                data.IsPaymentSuccessful = true;
                data.Status = Status.Booked;
                var updated = _db.CarOrderDetails.Update(data);
                await _db.SaveChangesAsync();

                return _mapper.Map<CarOrderDetails, CarOrderDetailsDTO>(updated.Entity);
            }
            return new CarOrderDetailsDTO();
        }

        // 264. В CarOrderDetailsRepository имплементировать недостающий метод UpdateOrderStatusAsync
        /// <summary>
        /// Updates the status of a car order.
        /// </summary>
        /// <param name="carDetailsId">The car details identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>
        ///   <c>true</c> if the status was successfully updated; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateOrderStatusAsync(int carDetailsId, Status status)
        {
            try
            {
                var carOrder = await _db.CarOrderDetails.FindAsync(carDetailsId);
                if (carOrder is null)
                    return false;

                carOrder.Status = status;

                if (status == Status.RentStarted)
                    carOrder.ActualStartRentDate = DateTime.Now;
                else if (status == Status.RenEnded)
                    carOrder.ActualEndRentDate = DateTime.Now;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
