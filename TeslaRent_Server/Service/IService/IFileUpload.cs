using Microsoft.AspNetCore.Components.Forms;

namespace TeslaRent_Server.Service.IService
{
    // 58. Создаём интерфейс для работы с загрузками картинок
    public interface IFileUpload
    {
        Task<string> UploadFileAsync(IBrowserFile file);
        bool DeleteFile(string fileName);
    }
}
