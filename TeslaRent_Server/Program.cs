using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Business.Repository.IRepository;
using Business.Repository;
using TeslaRent_Server.Service.IService;
using TeslaRent_Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 10. Внедряем конфигурацию базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 23. Внедряем зависимость AutoMapper в наше приложение
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 30. Внедряем и конфигурируем сервис, добавил ссылку на проект
builder.Services.AddScoped<ITeslaCarRepository, TeslaCarRepository>();

// 62.3. Конфигурируем зависимость загрузчика и класс загрузки
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddScoped<ITeslaCarImageRepository, TeslaCarImageRepository>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
