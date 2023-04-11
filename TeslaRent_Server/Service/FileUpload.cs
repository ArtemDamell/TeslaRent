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
        /// <summary>
        /// Deletes a file from the CarImages folder
        /// </summary>
        /// <param name="fileName">The name of the file to delete</param>
        /// <returns>True if the file was deleted, false otherwise</returns>
        public bool DeleteFile(string fileName)
        {
            try
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "CarImages", fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error image deleting.");
            }
        }

        // 59.2 Реализовываем метод загрузки изображений
        /// <summary>
        /// Uploads a file to the CarImages folder and returns the absolute path of the file.
        /// </summary>
        /// <returns>
        /// The absolute path of the uploaded file.
        /// </returns>
        public async Task<string> UploadFileAsync(IBrowserFile file)
        {
            try
            {
                long maxFileSize = 1024 * 1024 * 55;
                FileInfo fileInfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\CarImages";
                var path = Path.Combine(folderDirectory, fileName);

                if (!Directory.Exists(folderDirectory))
                    Directory.CreateDirectory(folderDirectory);

                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                }

                // 92.2 Составляем абсолютный путь
                var url = $"{_httpContextAccessor?.HttpContext?.Request.Scheme}://{_httpContextAccessor?.HttpContext?.Request.Host.Value}/";
                var fullPath = $"{url}CarImages/{fileName}";
                return fullPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Image upload error.");
            }
        }
    }
}
