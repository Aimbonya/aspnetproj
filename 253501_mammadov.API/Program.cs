using Microsoft.EntityFrameworkCore;
using _253501_mammadov.API.Data;
using System.Diagnostics;
using _253501_mammadov.API.Services;
using _253501_mammadov.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlite(connectionString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFruitService, FruitService>();

var authServer = builder.Configuration
.GetSection("AuthServer")
.Get<AuthServerData>();
// �������� ������ ��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    // ����� ���������� ������������ OpenID
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
    // Authority ������� ��������������
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
    // Audience ��� ������ JWT
    o.Audience = "account";
    // ��������� HTTPS ��� ������������� ��������� ������ Keycloak
    // � ������� ������� ������ ���� true
    o.RequireHttpsMetadata = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7131")  // ��������� ����������� ��������
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

app.UseStaticFiles();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
