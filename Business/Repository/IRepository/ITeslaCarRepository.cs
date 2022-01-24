using Models;

namespace Business.Repository.IRepository
{
    // 18. Создаём интерфейс
    public interface ITeslaCarRepository
    {
        // 18.1 Добавить ссылку на проект Models
        Task<TeslaCarDTO> CreateCar(TeslaCarDTO carForCreation);
        Task<TeslaCarDTO> UpdateCar(int carId, TeslaCarDTO carForUpdating);
        Task<int> DeleteCar(int carId);
        // 165.2 Добавить в метод GetAllCars новые параметры string startRentDate, string endRentDate
        Task<TeslaCarDTO> GetCar(int carId, string? startRentDate = null, string? endRentDate = null);
        // 165.1 Добавить в метод GetAllCars новые параметры string startRentDate, string endRentDate
        Task<IEnumerable<TeslaCarDTO>> GetAllCars(string? startRentDate = null, string? endRentDate = null);

        // 49.1 Добавляем параметр int carId = 0
        Task<TeslaCarDTO> IsCarUnique(string carName, int carId = 0);
        Task<IEnumerable<CarAccessoryDTO>> GetAllCarAccessories();
        Task<CarAccessoryDTO> GetSingleAccessory(int id);
    }
}
