using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using mammadov.Domain.Entities;
using _253501_mammadov.Services.ProductService;
using _253501_mammadov.Services.CategoryService;


namespace _253501_mammadov.Admin.Pages
{
    
    public class EditModel : PageModel
    {
        private readonly IFruitService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public IFormFile? UploadFile { get; set; }
        public EditModel(IFruitService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Fruit fruit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data == null)
            {
                return NotFound();
            }

            fruit = response.Data;

            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
            return Page();
        }


        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Валидация модели не пройдена");

                return RedirectToPage("./Index");
            }

            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(fruit.Id);
                if (existingProduct.Data == null)
                {
                    return NotFound();
                }

                if (Image != null)
                {
                    // Путь для сохранения загружаемого изображения
                    var uploadPath = Path.Combine(_environment.WebRootPath, "images");
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Image.FileName)}";
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Создаем каталог, если его нет
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Сохраняем файл на диск
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Обновляем данные о файле изображения
                    fruit.Image = $"/images/{uniqueFileName}";
                    fruit.ImageMimeType = Image.ContentType;
                }
                else
                {
                    // Если изображение не загружено, используем данные существующего продукта
                    fruit.Image = existingProduct.Data.Image;
                    fruit.ImageMimeType = existingProduct.Data.ImageMimeType;
                }

                // Обновляем продукт через сервис
                await _productService.UpdateProductAsync(fruit.Id, fruit, Image);
            }
            catch (Exception)
            {
                if (!await DetailExists(fruit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }



        private async Task<bool> DetailExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Data != null;
        }
    }
}
