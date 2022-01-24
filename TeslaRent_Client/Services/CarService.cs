using Models;
using Newtonsoft.Json;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Services
{
    // 164. Создать класс реализации CarService интерфейса ICarService
    public class CarService : ICarService
    {
        // 164.1 Для выполнения запросов к API сервису, внедряем зависимость httpClient
        private readonly HttpClient _client;
        public CarService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<TeslaCarDTO>> GetAllExistingCars(string startRentDate, string endRentDate)
        {
            var response = await _client.GetAsync($"api/teslacar?startRentDate={startRentDate}&endRentDate={endRentDate}");
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<IEnumerable<TeslaCarDTO>>(content);
            return cars;
        }

        public Task<TeslaCarDTO> GetCarDetails(int carId, string startRentDate, string endRentDate)
        {
            throw new NotImplementedException();
        }
    }
}
