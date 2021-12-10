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
        Task<TeslaCarDTO> GetCar(int carId);
        Task<IEnumerable<TeslaCarDTO>> GetAllCars();

        // 49.1 Добавляем параметр int carId = 0
        Task<TeslaCarDTO> IsCarUnique(string carName, int carId = 0);
        Task<IEnumerable<CarAccessoryDTO>> GetAllCarAccessories();
        Task<CarAccessoryDTO> GetSingleAccessory(int id);
    }
}
