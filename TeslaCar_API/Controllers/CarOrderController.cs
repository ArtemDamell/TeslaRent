using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
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

        // 253.1 Внести изменения в CarOrderController, добавив логику отправки почты в методе PaymetSeccessful
        private readonly IEmailSender _emailSender;

        public CarOrderController(ICarOrderDetailsRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        [HttpPost]
        /// <summary>
        /// Creates a car order based on the details provided in the request body.
        /// </summary>
        /// <param name="details">The details of the car order.</param>
        /// <returns>The result of the car order creation.</returns>
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
        /// <summary>
        /// Marks the payment as successful and sends an email to the customer.
        /// </summary>
        /// <param name="details">The details of the car order.</param>
        /// <returns>
        /// Returns an Ok result if the payment was successful, or a BadRequest if the payment was unsuccessful.
        /// </returns>
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

                // 253.2 Внести изменения в CarOrderController, добавив логику отправки почты в методе PaymetSeccessful
                await _emailSender.SendEmailAsync(details.Email, "Booking Confirmed! - Tesla Car Rent",
                    $"Your booking has been confirmed at Tesla Rent Service with ID: <b style=\"color: red\">{details.Id}</b>");
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
