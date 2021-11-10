using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    // 56. Добавить новый репозиторий для обработки изображений
    public interface ITeslaCarImageRepository
    {
        // 56.1
        public Task<bool> CreateTeslaCarImage(TeslaCarImageDTO image);
        // 56.2
        public Task<bool> DeleteTeslaCarImageByImageId(int imageId);
        // 56.3
        public Task<bool> DeleteTeslaCarImageByCarId(int carId);
        // 56.4
        public Task<IEnumerable<TeslaCarImageDTO>?> GetTeslaCarImages(int carId);
    }
}
