using Models;
using Models.DTO;
using Newtonsoft.Json;
using System.Text;
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

        // 185.4 Эти 2 метода пропускаем и реализуем их позже
        public async Task<CarOrderDetailsDTO> MarkPaymentSuccessful(CarOrderDetailsDTO details)
        {
            //213.Переходим в CarOrderDetailsService и реализовываем метод MarkPaymentSuccessful
            //throw new NotImplementedException();

            var content = JsonConvert.SerializeObject(details);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/carorder/paymentsuccessful", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CarOrderDetailsDTO>(tempContent);
                return result;
            }
            else
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorModel>(tempContent);
                Exception exception = new(error?.ErrorMessage);
                throw exception;
            }
        }

        public async Task<CarOrderDetailsDTO> SaveCarOrderDetails(CarOrderDetailsDTO details)
        {
            // 210.2 Временно решаем проблему с UserId, присвоив его в ручном режиме
            details.UserId = "dummy user";

            // 201. Идём в класс CarOrderDetailsService и реализовываем метод SaveCarOrderDetails
            var content = JsonConvert.SerializeObject(details);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/carorder/create", bodyContent);

            // 210.1 Для отлова ошибки переходим в CarOrderDetailsService и добавляем строку и точку останова
            //string res = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CarOrderDetailsDTO>(tempContent);
                return result;
            }
            else
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorModel>(tempContent);
                Exception exception = new(error?.ErrorMessage);
                throw exception;
            }
        }
    }
}
