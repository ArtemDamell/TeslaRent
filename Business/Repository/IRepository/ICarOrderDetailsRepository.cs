using Common;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    // 182. Создать новый интерфейс ICarOrderDetailsRepository
    public interface ICarOrderDetailsRepository
    {
        Task<CarOrderDetailsDTO> CreateAsync(CarOrderDetailsDTO carDetails);
        Task<CarOrderDetailsDTO> MarkPaymentSuccessfulAsync(int id);
        Task<CarOrderDetailsDTO> GetCarOrderDetailsAsync(int carOrderId);
        Task<IEnumerable<CarOrderDetailsDTO>> GetAllCarOrderDetailsAsync();
        Task<bool> UpdateOrderStatusAsync(int carDetailsId, Status status);
        // 217.3 Перенести метод IsCarBookedAsync из CarOrderDetailsRepository в TeslaCarRepository
        //Task<bool> IsCarBookedAsync(int carId, DateTime startRentDate, DateTime endRentDate);
    }
}
