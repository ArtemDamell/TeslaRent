using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TeslaRent_Client;
using TeslaRent_Client.Services;
using TeslaRent_Client.Services.IServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 168.2 �������� � ������ Program TeslaRent_Client ������� ������� URL API
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl"))});

// 158. �������� ����������� � ����� Program ������� TeslaRent_Client
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ICarService, CarService>();

// 186. ������������ ����� ������ � ������ Program TeslaRent_Client
builder.Services.AddScoped<ICarOrderDetailsService, CarOrderDetailsService>();

// 204. ���������������� ����������� ������� IStripePaymentService � ������ Program ������� Client
builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();

await builder.Build().RunAsync();
