using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Stripe.Checkout;

namespace TeslaCar_API.Controllers
{
    // 195. Создать новый контроллер в проекте API StripePaymentController
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StripePaymentController : Controller
    {
        // 198. Переходим к реализации StripePaymentController
        private readonly IConfiguration _configuration;
        public StripePaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create(StripePaymentDTO payment)
        {
            try
            {
                var domain = _configuration.GetValue<string>("TeslaCar_Client_URL");
                // На этом этапе идём в NuGet и устанавливаем пакет Stripe.Net

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = payment.Amount * 100, // Конвертируем в центы
                                Currency = "eur",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = payment.ProductName
                                }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = $"{domain}/success-payment?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{domain}{payment.ReturnUrl}"
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return Ok(new SuccessModel()
                {
                    Data = session.Id // <-- По этому ID можно получить всю информацию о заказе и транзакции, большего и не надо
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = 400,
                    Title="Create Payment failed"
                });
            }
            
        }
    }
}
