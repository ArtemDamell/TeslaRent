using Microsoft.JSInterop;

namespace TeslaRent_Server.Helpers
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
