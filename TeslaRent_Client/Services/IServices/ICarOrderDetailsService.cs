using Models.DTO;

namespace TeslaRent_Client.Services.IServices
{
    // 185.1 Добавить новый сервис в Services проекта TeslaRent_Client
    public interface ICarOrderDetailsService
    {
        Task<CarOrderDetailsDTO> SaveCarOrderDetails(CarOrderDetailsDTO details);
        Task<CarOrderDetailsDTO> MarkPaymentSuccessful(CarOrderDetailsDTO details);
    }
}
