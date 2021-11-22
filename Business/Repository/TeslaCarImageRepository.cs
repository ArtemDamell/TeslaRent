using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Business.Repository
{
    // 57. Реализовываем интерфейс работы с картинками
    public class TeslaCarImageRepository : ITeslaCarImageRepository
    {
        // 57.1 Внедряем зависимость базы и маппера
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public TeslaCarImageRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> CreateTeslaCarImage(TeslaCarImageDTO imageDTO)
        {
            try
            {
                var image = _mapper.Map<TeslaCarImageDTO, TeslaCarImage>(imageDTO);
                await _db.TeslaCarImages.AddAsync(image);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTeslaCarImageByCarId(int carId)
        {
            try
            {
                var imageList = await _db.TeslaCarImages.Where(x => x.CarId == carId).ToListAsync();
                _db.TeslaCarImages.RemoveRange(imageList);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTeslaCarImageByImageId(int imageId)
        {
            try
            {
                var image = await _db.TeslaCarImages.FindAsync(imageId);
                _db.TeslaCarImages.Remove(image);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 70.2
        public async Task<bool> DeleteTeslaCarImageByImageUrl(string imageUrl)
        {
            try
            {
                var allImages = await _db.TeslaCarImages.FirstOrDefaultAsync(x => x.CarImageUrl.ToLower().Equals(imageUrl.ToLower()));

                _db.TeslaCarImages.Remove(allImages);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IEnumerable<TeslaCarImageDTO>?> GetTeslaCarImages(int carId)
        {
            try
            {
                var imagesList = await _db.TeslaCarImages.Where(x => x.CarId == carId).ToListAsync();
                return _mapper.Map<IEnumerable<TeslaCarImage>, IEnumerable<TeslaCarImageDTO>>(imagesList);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
