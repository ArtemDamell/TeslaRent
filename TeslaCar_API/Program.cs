using Business.Repository.IRepository;
using Business.Repository;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 120.1 Строка подключения
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});

// 120.2 Строка подключения
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 120.3 Конфигурируем зависимости
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // <-- Добавить библиотеку AutoMapper из Nuget
builder.Services.AddScoped<ITeslaCarRepository, TeslaCarRepository>();
builder.Services.AddScoped<ITeslaCarImageRepository, TeslaCarImageRepository>();
builder.Services.AddScoped<ITeslaCarAccessoryRepository, TeslaCarAccessoryRepository>();

// 120.4 Конфигурируем маршруты так, чтобы они всегда были маленькими буквами
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
