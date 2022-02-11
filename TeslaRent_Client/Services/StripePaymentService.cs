using Models;
using Models.DTO;
using Newtonsoft.Json;
using System.Text;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Services
{
    // 200. Имплементируем интерфейс в класс реализации StripePaymentService
    public class StripePaymentService : IStripePaymentService
    {
        private readonly HttpClient _client;

        public StripePaymentService(HttpClient client)
        {
            _client = client;
        }
        public async Task<SuccessModel> CheckOut(StripePaymentDTO info)
        {
            var content = JsonConvert.SerializeObject(info);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/StripePayment/Create", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SuccessModel>(tempContent);
                return result;
            }
            else
            {
                var tempContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorModel>(tempContent);
                throw new Exception(error.ErrorMessage);
            }
        }
    }
}
