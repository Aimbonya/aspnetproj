using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mammadov.Domain.Entities;
using _253501_mammadov.Services.ProductService;

namespace _253501_mammadov.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFruitService _productService;

        public IndexModel(IFruitService productService)
        {
            _productService = productService;
        }

        public List<Fruit> Products { get; set; } = new List<Fruit>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            CurrentPage = pageNo;

            var response = await _productService.GetProductListAsync(null, pageNo);

            if (response.Data != null)
            {
                Products = response.Data?.Items ?? new List<Fruit>();
                TotalPages = response.Data?.TotalPages ?? 0;
            }
        }
    }
}
