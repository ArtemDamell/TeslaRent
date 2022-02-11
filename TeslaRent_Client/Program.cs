using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TeslaRent_Client;
using TeslaRent_Client.Services;
using TeslaRent_Client.Services.IServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 168.2 заменяем в классе Program TeslaRent_Client проекта базовый URL API
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl"))});

// 158. Внедрить зависимость в класс Program проекта TeslaRent_Client
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ICarService, CarService>();

// 186. Регистрируем новый сервис в классе Program TeslaRent_Client
builder.Services.AddScoped<ICarOrderDetailsService, CarOrderDetailsService>();

// 204. Сконфигурировать зависимость сервиса IStripePaymentService в классе Program проекта Client
builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();

await builder.Build().RunAsync();
