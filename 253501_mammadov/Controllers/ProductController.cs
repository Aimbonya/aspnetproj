using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.ProductService;
using mammadov.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace _253501_mammadov.Controllers
{
    public class ProductController : Controller
    {
        private IFruitService _fruitService;
        private ICategoryService _categoryService;

        public ProductController(ICategoryService categoryService, IFruitService fruitService)
        {
            _fruitService = fruitService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category = null, int pageNo = 1)
        {
            var response = await _categoryService.GetCategoryListAsync();
            var categories = response.Data;

            var currentCategory = categories?.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
                ViewData["categories"] = categories;
                ViewData["currentCategory"] = currentCategory;
                ViewData["currentCategoryNormalizedName"] = category;

            var products = await _fruitService.GetProductListAsync(category, pageNo);
            var model = products.Data;

            return View(model);
        }
    }
}



