using Microsoft.JSInterop;

// 151. Обязательно поменять название классов в скопированном Helper'е
namespace TeslaRent_Client.Helpers
{
    // 40. Создаём метод расширения для вызова toastr
    public static class JsRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
