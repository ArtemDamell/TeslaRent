using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Models;
using Microsoft.AspNetCore.Authorization;
using Common;

namespace TeslaCar_API.Controllers
{
    // 122.1 Создаём контроллер и прописываем к нему маршрут
    [ApiController]
    [Route("api/[controller]")]
    public class TeslaCarController : Controller
    {
        // 122.2 Внедряем зависимость репозитория машин
        private readonly ITeslaCarRepository _teslaCarRepository;
        public TeslaCarController(ITeslaCarRepository teslaCarRepository)
        {
            _teslaCarRepository = teslaCarRepository;
        }

        // 122.3 Создаём метод получения всех машин
        // 146.1 Закроем метод получения всех машин на обычную авторизацию [Authorize]
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var allCars = await _teslaCarRepository.GetAllCars();
            return Ok(allCars);
        }

        // --> На этом месте создаём ErrorModel в проекте Models
        // 146.2 Закроем метод получения получение машины по ID на авторизацию с ролью администратора
        [Authorize(Roles = SD.ADMIN_ROLE)]
        [HttpGet("{carId}")]
        public async Task<IActionResult> GetSingleTeslaCar(int? carId)
        {
            if (carId is null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "ID",
                    ErrorMessage = "Invalid car ID!",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var carDetails = await _teslaCarRepository.GetCar(carId.Value);

            if (carDetails is null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title="Car not found",
                    ErrorMessage = $"Car by id {carId} NOT FOUND!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(carDetails);
        }
    }
}
