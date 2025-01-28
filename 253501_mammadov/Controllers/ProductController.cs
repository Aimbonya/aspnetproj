using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.ProductService;
using _253501_mammadov.Extensions;
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
            var products = await _fruitService.GetProductListAsync(category, pageNo);

            if (!response.Successful || !products.Successful)
            {
                return NotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FruitList", products.Data);
            }

            var categories = response.Data;
            var currentCategory = categories?.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";

            ViewData["categories"] = categories;
            ViewData["currentCategory"] = currentCategory;
            ViewData["currentCategoryNormalizedName"] = category;

            var model = products.Data;
            return View(model);
        }

    }
}



