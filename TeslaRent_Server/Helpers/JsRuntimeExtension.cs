using Microsoft.JSInterop;

namespace TeslaRent_Server.Helpers
{
    // 40. Создаём метод расширения для вызова toastr
    public static class JsRuntimeExtension
    {
        /// <summary>
        /// Invokes a JavaScript function to show a success toastr message.
        /// </summary>
        /// <param name="js">The IJSRuntime instance.</param>
        /// <param name="message">The message to show.</param>
        /// <returns>A ValueTask representing the asynchronous operation.</returns>
        public static ValueTask ToastrSuccess(this IJSRuntime js, string message)
        {
            return js.InvokeVoidAsync("ShowToastr", "success", message);
        }

        /// <summary>
        /// Invokes the ShowToastr JavaScript function with the error type and the specified message.
        /// </summary>
        /// <param name="js">The IJSRuntime instance.</param>
        /// <param name="message">The message to be displayed.</param>
        /// <returns>A ValueTask representing the asynchronous operation.</returns>
        public static ValueTask ToastrError(this IJSRuntime js, string message)
        {
            return js.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
