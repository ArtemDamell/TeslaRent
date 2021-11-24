using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface ITeslaCarAccessoryRepository
    {
        Task<IEnumerable<CarAccessoryDTO>> GetAllAccessoriesAsync();
        Task<CarAccessoryDTO> GetSingleAccessoryAsync(int accessoryId);
        Task<CarAccessoryDTO> CreateAccessoryAsync(CarAccessoryDTO newAccessory);
        Task<CarAccessoryDTO> UpdateAccessoryAsync(CarAccessoryDTO updatedAccessory);
        Task<bool> DeleteAccessoryAsync(int accessoryId);
    }
}
