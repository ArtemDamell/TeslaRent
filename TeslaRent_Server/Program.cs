using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Business.Repository.IRepository;
using Business.Repository;
using TeslaRent_Server.Service.IService;
using TeslaRent_Server.Service;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging(loggingBuilder => {
//    loggingBuilder.AddConsole()
//        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
//    loggingBuilder.AddDebug();
//});

// Add services to the container.
// 10. �������� ������������ ���� ������
builder.Services.AddDbContext<ApplicationDbContext>(options => { 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        //options.EnableSensitiveDataLogging(true);
});
// 99. В последней версии .NET это сделалось автоматически, но тут нет ролей! ПЕРЕПИСЫВАЕМ!!!
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer("ConnectionString"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// 23. �������� ����������� AutoMapper � ���� ����������
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 30. �������� � ������������� ������, ������� ������ �� ������
builder.Services.AddScoped<ITeslaCarRepository, TeslaCarRepository>();

// 62.3. ������������� ����������� ���������� � ����� ��������
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddScoped<ITeslaCarImageRepository, TeslaCarImageRepository>();

// 86. ���������������
builder.Services.AddScoped<ITeslaCarAccessoryRepository, TeslaCarAccessoryRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// 91. �������� ����������� HttpContexntAccessor
builder.Services.AddHttpContextAccessor();

// 115.1 Внедряим конфигурацию сервиса инициализации базы данных
//builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<ICarOrderDetailsRepository, CarOrderDetailsRepository>();

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

// 94.1 Конфигурируем систему Identity
app.UseAuthentication();
app.UseAuthorization();

// 94.2 Конфигурируем систему Identity
app.MapRazorPages();

// 115.2 Внедряем Middleware для запуска класса инициализации
//app.UseDbInitializer();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
