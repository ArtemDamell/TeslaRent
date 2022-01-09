using Microsoft.AspNetCore.Components.Forms;
using TeslaRent_Server.Service.IService;

namespace TeslaRent_Server.Service
{
    // 59. Создать класс реализации FileUpload
    public class FileUpload : IFileUpload
    {
        // 59.1 Чтобы использовать папку wwwroot нам необходимо внедрить зависимость
        private readonly IWebHostEnvironment _webHostEnvironment;

        // 92.1 Внедряем зависимость HttpContextAccessor
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        // 59.3 Реализовываем метод удаления картинок
        public bool DeleteFile(string fileName)
        {
            try
            {
                var path = $"{_webHostEnvironment.WebRootPath}\\CarImages\\{fileName}";

                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 59.2 Реализовываем метод загрузки изображений
        public async Task<string> UploadFileAsync(IBrowserFile file)
        {
            try
            {
                long maxFileSize = 1024 * 1024 * 55;
                FileInfo fileInfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\CarImages";
                var path = Path.Combine(folderDirectory, fileName);

                var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);

                if (!Directory.Exists(folderDirectory))
                    Directory.CreateDirectory(folderDirectory);

                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }

                // 92.2 Составляем абсолютный путь
                var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                var fullPath = $"{url}CarImages/{fileName}";
                return fullPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
