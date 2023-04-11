using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Globalization;

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
        // 166.1 Перейти в контроллер TeslaCarController и изменить методы получения машин(GetAllCars, GetCar)
        [HttpGet]
        /// <summary>
        /// Gets all cars with specified start and end rent dates.
        /// </summary>
        /// <param name="startRentDate">Start rent date in MM.dd.yyyy format.</param>
        /// <param name="endRentDate">End rent date in MM.dd.yyyy format.</param>
        /// <returns>List of cars.</returns>
        public async Task<IActionResult> GetAllCars(string? startRentDate = null, string? endRentDate = null)
        {
            // 166.3 Изменяем метод под новые параметры *******************************************
            if (string.IsNullOrWhiteSpace(startRentDate) || string.IsNullOrWhiteSpace(endRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "No parameters setted",
                    ErrorMessage = "All parameters need to be supplied",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            if (!DateTime.TryParseExact(startRentDate, "MM.dd.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtStartRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Invalid Start Rent Date format",
                    ErrorMessage = "Invalid Start Rent Date format. Valid format will be MM.dd.yyyy",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            if (!DateTime.TryParseExact(endRentDate, "MM.dd.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtEndRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Invalid End Rent Date format",
                    ErrorMessage = "Invalid End Rent Date format. Valid format will be MM.dd.yyyy",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            // ******************************************************************************************

            // 166.4 Добавляем параметры в метод *******************************************
            var allCars = await _teslaCarRepository.GetAllCars(startRentDate, endRentDate);
            return Ok(allCars);
        }

        // --> На этом месте создаём ErrorModel в проекте Models
        // 146.2 Закроем метод получения получение машины по ID на авторизацию с ролью администратора
        //[Authorize(Roles = SD.ADMIN_ROLE)]
        // 166.2 Перейти в контроллер TeslaCarController и изменить методы получения машин(GetAllCars, GetCar)
        [HttpGet("{carId}")]
        /// <summary>
        /// Gets a single Tesla car with the specified carId, startRentDate and endRentDate.
        /// </summary>
        /// <param name="carId">The carId of the Tesla car.</param>
        /// <param name="startRentDate">The start rent date of the Tesla car.</param>
        /// <param name="endRentDate">The end rent date of the Tesla car.</param>
        /// <returns>The Tesla car with the specified carId, startRentDate and endRentDate.</returns>
        public async Task<IActionResult> GetSingleTeslaCar(int? carId, string? startRentDate = null, string? endRentDate = null)
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

            // 166.5 Изменяем метод под новые параметры *******************************************
            if (string.IsNullOrWhiteSpace(startRentDate) || string.IsNullOrWhiteSpace(endRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "No parameters setted",
                    ErrorMessage = "All parameters need to be supplied",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            if (!DateTime.TryParseExact(startRentDate, "MM.dd.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtStartRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Invalid Start Rent Date format",
                    ErrorMessage = "Invalid Start Rent Date format. Valid format will be MM.dd.yyyy",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            if (!DateTime.TryParseExact(endRentDate, "MM.dd.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtEndRentDate))
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Invalid End Rent Date format",
                    ErrorMessage = "Invalid End Rent Date format. Valid format will be MM.dd.yyyy",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            // ******************************************************************************************

            // 166.6 Добавляем параметры в метод *******************************************
            var carDetails = await _teslaCarRepository.GetCar(carId.Value, startRentDate, endRentDate);

            if (carDetails is null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Car not found",
                    ErrorMessage = $"Car by id {carId} NOT FOUND!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(carDetails);
        }
    }
}
