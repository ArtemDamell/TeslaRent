﻿using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Create new car order
        /// </summary>
        /// <param name="carDetails">Car Order Details DTO model</param>
        /// <returns>DTO model or NULL</returns>
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

                return null;
            }
        }

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

                return null;
            }
        }

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

                return null;
            }
        }

        public async Task<bool> IsCarBookedAsync(int carId, DateTime startRentDate, DateTime endRentDate)
        {
            var bookingStatus = false;
            var existingBooking = await _db.CarOrderDetails.Where(x => 
                                                                                     x.CarId == carId &&
                                                                                     x.IsPaymentSuccessful &&
                                                                                     startRentDate < x.EndRentDate &&
                                                                                     endRentDate.Date > x.StartRentDate ||
                                                                                     endRentDate.Date > x.StartRentDate.Date &&
                                                                                     startRentDate.Date < x.StartRentDate.Date).FirstOrDefaultAsync();

            if (existingBooking is not null)
                bookingStatus = true;

            return bookingStatus;
        }

        // Эти 2 метода реализуем позже, не на шаге 183!
        public Task<CarOrderDetailsDTO> MarkPaymentSuccessfulAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderStatusAsync(int carDetailsId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
