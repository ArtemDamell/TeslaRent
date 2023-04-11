using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;

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

        /// <summary>
        /// Creates a new car accessory and returns the created car accessory.
        /// </summary>
        /// <returns>
        /// The created car accessory.
        /// </returns>
        public async Task<CarAccessoryDTO> CreateAccessoryAsync(CarAccessoryDTO newAccessory)
        {
            try
            {
                var carForCreation = _mapper.Map<CarAccessoryDTO, CarAccessory>(newAccessory);
                carForCreation.CreatedDate = DateTime.UtcNow;
                carForCreation.CreatedBy = "";

                var createdCar = await _db.CarAccessories.AddAsync(carForCreation);
                await _db.SaveChangesAsync();

                return _mapper.Map<CarAccessory, CarAccessoryDTO>(createdCar.Entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes a car accessory from the database.
        /// </summary>
        /// <param name="accessoryId">The ID of the car accessory to delete.</param>
        /// <returns>True if the car accessory was successfully deleted, false otherwise.</returns>
        public async Task<bool> DeleteAccessoryAsync(int accessoryId)
        {
            try
            {
                var accessoryForDeleting = await _db.CarAccessories.FindAsync(accessoryId);

                _db.CarAccessories.Remove(accessoryForDeleting);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves all car accessories from the database asynchronously.
        /// </summary>
        /// <returns>
        /// An enumerable of CarAccessoryDTO objects.
        /// </returns>
        public async Task<IEnumerable<CarAccessoryDTO>> GetAllAccessoriesAsync()
        {
            try
            {
                var acc = await _db.CarAccessories.AsNoTracking().ToListAsync();
                var allAccessories = _mapper.Map<IEnumerable<CarAccessory>, IEnumerable<CarAccessoryDTO>>(acc);
                return allAccessories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a single car accessory from the database.
        /// </summary>
        /// <param name="accessoryId">The ID of the car accessory to retrieve.</param>
        /// <returns>A CarAccessoryDTO object containing the car accessory data.</returns>
        public async Task<CarAccessoryDTO> GetSingleAccessoryAsync(int accessoryId)
        {
            try
            {
                var accessory = await _db.CarAccessories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == accessoryId);

                return _mapper.Map<CarAccessory, CarAccessoryDTO>(accessory);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates a car accessory in the database.
        /// </summary>
        /// <param name="updatedAccessory">The updated car accessory.</param>
        /// <returns>The updated car accessory.</returns>
        public async Task<CarAccessoryDTO> UpdateAccessoryAsync(CarAccessoryDTO updatedAccessory)
        {
            try
            {
                var accessoryForUpdating = _mapper.Map<CarAccessoryDTO, CarAccessory>(updatedAccessory);
                accessoryForUpdating.UpdatedDate = DateTime.UtcNow;
                accessoryForUpdating.UpdatedBy = "";


                var result = _db.Update(accessoryForUpdating);
                await _db.SaveChangesAsync();

                return _mapper.Map<CarAccessory, CarAccessoryDTO>(result.Entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
