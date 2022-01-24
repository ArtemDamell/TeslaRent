using Models;

namespace TeslaRent_Client.Services.IServices
{
    // 163. В проекте TeslaRent_Client создать папку Services, в ней папку IServices, в ней ICarService
    public interface ICarService
    {
        Task<IEnumerable<TeslaCarDTO>> GetAllExistingCars(string startRentDate, string endRentDate);
        Task<TeslaCarDTO> GetCarDetails(int carId, string startRentDate, string endRentDate);
    }
}
