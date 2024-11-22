using _253501_mammadov.HelperClasses;
using _253501_mammadov.Services.Authentication;
using _253501_mammadov.Services.Authorization;
using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.FileService;
using _253501_mammadov.Services.ProductService;

namespace _253501_mammadov.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
            builder.Services.AddScoped<IFruitService, ApiProductService>();
            builder.Services.AddScoped<IFileService, ApiFileService>();
            builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
            builder.Services
                            .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<KeycloakTokenAccessor>();
            
        }
    }
}
