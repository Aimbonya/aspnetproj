using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using mammadov.Domain.Entities;
using _253501_mammadov.Services.ProductService;

namespace _253501_mammadov.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IFruitService _fruitService;

        public DeleteModel(IFruitService productService)
        {
            _fruitService = productService;
        }

        [BindProperty]
        public Fruit Fruit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _fruitService.GetProductByIdAsync(id.Value);
            if (!response.Successful || response.Data == null)
            {
                return NotFound();
            }

            Fruit = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _fruitService.DeleteProductAsync(id.Value);
            if (result.Successful)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Не удалось удалить продукт. Попробуйте позже.");
            return Page();
        }
    }
}
