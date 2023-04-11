using Microsoft.JSInterop;

// 151. Обязательно поменять название классов в скопированном Helper'е
namespace TeslaRent_Client.Helpers
{
    // 40. Создаём метод расширения для вызова toastr
    public static class JsRuntimeExtension
    {
        /// <summary>
        /// Invokes the ShowToastr JavaScript function with the success type and the given message.
        /// </summary>
        /// <param name="js">The IJSRuntime instance.</param>
        /// <param name="message">The message to be displayed.</param>
        /// <returns>A ValueTask representing the asynchronous operation.</returns>
        public static ValueTask ToastrSuccess(this IJSRuntime js, string message)
        {
            return js.InvokeVoidAsync("ShowToastr", "success", message);
        }

        /// <summary>
        /// Invokes the ShowToastr JavaScript function with the error type and message.
        /// </summary>
        /// <param name="js">The IJSRuntime instance.</param>
        /// <param name="message">The message to display.</param>
        /// <returns>A ValueTask representing the asynchronous operation.</returns>
        public static ValueTask ToastrError(this IJSRuntime js, string message)
        {
            return js.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
