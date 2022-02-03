using Models.DTO;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Services
{
    // 185.2 Добавить новый сервис в Services проекта TeslaRent_Client
    public class CarOrderDetailsService : ICarOrderDetailsService
    {
        // 185.3
        private readonly HttpClient _client;
        public CarOrderDetailsService(HttpClient client)
        {
            _client = client;
        }
        public Task<CarOrderDetailsDTO> MarkPaymentSuccessful(CarOrderDetailsDTO details)
        {
            throw new NotImplementedException();
        }

        public Task<CarOrderDetailsDTO> SaveCarOrderDetails(CarOrderDetailsDTO details)
        {
            throw new NotImplementedException();
        }
    }
}
