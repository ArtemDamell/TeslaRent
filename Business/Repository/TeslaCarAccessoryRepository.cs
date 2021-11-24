using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    //86. Домашнее задание
    public class TeslaCarAccessoryRepository : ITeslaCarAccessoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public TeslaCarAccessoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CarAccessoryDTO> CreateAccessoryAsync(CarAccessoryDTO newAccessory)
        {
            try
            {
                var carForCreation = _mapper.Map<CarAccessoryDTO, CarAccessory>(newAccessory);
                carForCreation.CreatedDate = DateTime.UtcNow;
                carForCreation.CreatedBy = "";

                var createdCar = await _db.AddAsync(carForCreation);
                await _db.SaveChangesAsync();

                return _mapper.Map<CarAccessory, CarAccessoryDTO>(createdCar.Entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAccessoryAsync(int accessoryId)
        {
            try
            {
                var accessoryForDeleting = await _db.CarAccessories.FindAsync(accessoryId);

                _db.Remove(accessoryForDeleting);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<CarAccessoryDTO>> GetAllAccessoriesAsync()
        {
            try
            {
                var allAccessories = _mapper.Map<IEnumerable<CarAccessory>, IEnumerable< CarAccessoryDTO>>(await _db.CarAccessories.ToListAsync());
                return allAccessories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CarAccessoryDTO> GetSingleAccessoryAsync(int accessoryId)
        {
            try
            {
                return _mapper.Map<CarAccessory, CarAccessoryDTO>(await _db.CarAccessories.FindAsync(accessoryId));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<CarAccessoryDTO> UpdateAccessoryAsync(CarAccessoryDTO updatedAccessory)
        {
            try
            {
                var accessoryForUpdating = _mapper.Map<CarAccessoryDTO, CarAccessory>(updatedAccessory);
                accessoryForUpdating.UpdatedDate = DateTime.UtcNow;
                accessoryForUpdating.UpdatedBy = "";


                _db.CarAccessories.Update(accessoryForUpdating);
                await _db.SaveChangesAsync();

                return _mapper.Map<CarAccessory, CarAccessoryDTO>(accessoryForUpdating);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
