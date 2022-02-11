using Business.Repository.IRepository;
using Business.Repository;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeslaCar_API.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 120.1 ������ �����������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});

// 120.2 ������ �����������
//127.� ������ Program ��������� ������������ Identity � IdentityUser �� ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 136. ������������� ���������� �������� � ����� ����� � ������ Program
var appSettingsSection = builder.Configuration.GetSection("APISettings");
builder.Services.Configure<APISettings>(appSettingsSection);

// 140 ������������� �������������� ��� API. ��������� � ����� Program.
var apiSettings = appSettingsSection.Get<APISettings>();
var key = Encoding.ASCII.GetBytes(apiSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = apiSettings.ValidAudience,
        ValidIssuer = apiSettings.ValidIssuer,
        // ������� ����� ������� �� ������� ��������������� ����, �� ������ �� 0, �� ��������� - 5 �����
        // �� ���� ������ ������ ������� ����� ����
        ClockSkew = TimeSpan.Zero
    };
});

// 120.3 ������������� �����������
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // <-- �������� ���������� AutoMapper �� Nuget
builder.Services.AddScoped<ITeslaCarRepository, TeslaCarRepository>();
builder.Services.AddScoped<ITeslaCarImageRepository, TeslaCarImageRepository>();
builder.Services.AddScoped<ITeslaCarAccessoryRepository, TeslaCarAccessoryRepository>();

// 184. �������� ICarOrderDetailsRepository � ����� Program, ������� API
builder.Services.AddScoped<ICarOrderDetailsRepository, CarOrderDetailsRepository>();


// 141.1 ��������� ����� ����� ��������� �������� ������� � API � ������ Program API
builder.Services.AddCors(x => x.AddPolicy("TeslaRent", builder =>
{
    builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
}));


// 120.4 ������������� �������� ���, ����� ��� ������ ���� ���������� �������
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// 141.2 ������������ ����������� �� ������ � JSON ������� �� ���������
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
// 143. ���������� ������������ ������ � JSON
                                                .AddNewtonsoftJson(options =>
                                                {
                                                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                                                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                                                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 145. �������� Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tesla Rent API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
       }
   });

});

var app = builder.Build();

// 208. ��������� � middleware � ������ Program ������� API ������������� ���������� ����� Stripe
StripeConfiguration.ApiKey = app.Configuration.GetSection("Stripe")["ApiKey"];

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

// 144.1 �������� �������� ������������ � ������� Middleware
app.UseCors("TeslaRent");

// 144.2 �������� ���������� ��������������, ����� ������ ������ �� �����������
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
