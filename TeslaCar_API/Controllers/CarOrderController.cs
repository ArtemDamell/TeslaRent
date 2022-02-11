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
    }
}
