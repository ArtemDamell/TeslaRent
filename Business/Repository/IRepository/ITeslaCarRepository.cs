using Models;

namespace Business.Repository.IRepository
{
    // 18. Создаём интерфейс
    public interface ITeslaCarRepository
    {
        // 18.1 Добавить ссылку на проект Models
        public Task<TeslaCarDTO> CreateCar(TeslaCarDTO carForCreation);
        public Task<TeslaCarDTO> UpdateCar(int carId, TeslaCarDTO carForUpdating, List<CarAccessoryDTO> accessoriesForAdding);
        public Task<int> DeleteCar(int carId);
        public Task<TeslaCarDTO> GetCar(int carId);
        public Task<IEnumerable<TeslaCarDTO>> GetAllCars();

        // 49.1 Добавляем параметр int carId = 0
        public Task<TeslaCarDTO> IsCarUnique(string carName, int carId = 0);
    }
}
