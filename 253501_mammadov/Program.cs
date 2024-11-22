using _253501_mammadov.DataControllers;
using _253501_mammadov.Extensions;
using _253501_mammadov.HelperClasses;
using _253501_mammadov.Services.Authorization;
using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.FileService;
using _253501_mammadov.Services.ProductService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();

// Add services to the container.
builder.Services.AddControllersWithViews();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
builder.Services.AddHttpClient<IFruitService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
    opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
builder.Services.AddRazorPages();

var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();


builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
options.MetadataAddress =
$"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

var app = builder.Build();

List<string> errors = new List<string>();
errors.Count();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

