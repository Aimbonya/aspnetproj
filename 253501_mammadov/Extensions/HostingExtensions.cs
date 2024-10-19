using _253501_mammadov.Services.CategoryService;
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
        }
    }
}
