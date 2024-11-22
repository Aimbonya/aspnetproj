using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using mammadov.Domain.Entities;
using _253501_mammadov.Services.ProductService;
using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.FileService;
using NuGet.Protocol.Plugins;

namespace _253501_mammadov.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IFruitService _fruitService;
        private readonly ApiFileService _fileService;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(IFruitService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _fruitService = productService;
            _categoryService = categoryService;
            _environment = environment;
        }

        public async Task OnGetAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = response?.Data != null
                ? new SelectList(response.Data, "Id", "Name")
                : new SelectList(new List<Category>(), "Id", "Name");
        }

        [BindProperty]
        public Fruit fruit { get; set; } = default!;

        // Свойство для хранения загружаемого файла
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Поле: {state.Key}, Ошибка: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            try
            {
                if (Image != null)
                {
                    var uploadPath = Path.Combine(_environment.WebRootPath, "images");
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Image.FileName)}";
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    fruit.Image = $"/images/{uniqueFileName}";
                    fruit.ImageMimeType = Image.ContentType;
                }

                // Добавляем новый продукт через сервис
                await _fruitService.CreateProductAsync(fruit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
