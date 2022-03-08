using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;

namespace TeslaCar_API.Controllers
{
    // 202. Добавить новый контроллер CarOrderController в проект API
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarOrderController : ControllerBase
    {
        private readonly ICarOrderDetailsRepository _repository;

        public CarOrderController(ICarOrderDetailsRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarOrderDetailsDTO details)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.CreateAsync(details);
                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Error while creating car details/booking",
                    StatusCode = 400,
                    Title = "Booking/details saving error"
                });
            }
        }

        // 214. Реализовать новый метод в CarOrderController
        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful([FromBody] CarOrderDetailsDTO details)
        {
            var service = new Stripe.Checkout.SessionService();
            var sessionDetails = service.Get(details.StripeSessionId);

            if (sessionDetails.PaymentStatus.Equals("paid"))
            {
                var result = await _repository.MarkPaymentSuccessfulAsync(details.Id);
                if (result is null)
                {
                    return BadRequest(new ErrorModel()
                    {
                        ErrorMessage = "Can not mark payment as successful",
                        StatusCode = 400,
                        Title = "Mark as successful fail"
                    });
                }
                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Can not mark payment as successful",
                    StatusCode = 400,
                    Title = "Mark as successful fail"
                });
            }
        }
    }
}
