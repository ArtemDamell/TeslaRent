using Models;
using Models.DTO;

namespace TeslaRent_Client.Services.IServices
{
    // 199. Добавить в Services новый сервис IStripePaymentService
    public interface IStripePaymentService
    {
        Task<SuccessModel> CheckOut(StripePaymentDTO info);
    }
}
