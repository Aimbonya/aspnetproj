using _253501_mammadov.Models;
using _253501_mammadov.Services.CategoryService;
using mammadov.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace _253501_mammadov.Services.ProductService
{
    public class MemoryProductService : IFruitService
    {

        private readonly int _pageSize;
        List<Fruit> _fruits;
        List<Category> _categories;


        public MemoryProductService([FromServices] IConfiguration config,
                                    ICategoryService categoryService)
        {
            _pageSize = config.GetValue<int>("ItemsPerPage");
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }

        private void SetupData()
        {
        //    _fruits = new List<Fruit>
        //    {
        //       new Fruit
        //{
        //    Id = 1, Name = "Апельсин",
        //    Description = "Сочный цитрусовый фрукт",
        //    Price = 120, Image = "Images/orange.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("citrus-fruits"))
        //},
        //new Fruit
        //{
        //    Id = 2, Name = "Лимон",
        //    Description = "Кислый и ароматный фрукт",
        //    Price = 90, Image = "Images/lemon.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("citrus-fruits"))
        //},
        //new Fruit
        //{
        //    Id = 3, Name = "Грейпфрут",
        //    Description = "Фрукт с приятной горчинкой",
        //    Price = 140, Image = "Images/grapefruit.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("citrus-fruits"))
        //},

        //// Ягодные
        //new Fruit
        //{
        //    Id = 4, Name = "Клубника",
        //    Description = "Сладкая и сочная ягода",
        //    Price = 200, Image = "Images/strawberry.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("berries"))
        //},
        //new Fruit
        //{
        //    Id = 5, Name = "Малина",
        //    Description = "Мягкая ягода с насыщенным вкусом",
        //    Price = 250, Image = "Images/raspberry.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("berries"))
        //},
        //new Fruit
        //{
        //    Id = 6, Name = "Черника",
        //    Description = "Мягкая ягода с насыщенным вкусом",
        //    Price = 250, Image = "Images/raspberry.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("berries"))
        //},
        //    new Fruit {Id = 7, Name="Яблоко",
        //    Description="Красное яблоко, сладкое",
        //    Price = 80, Image="Images/kekw.png",
        //    Category = _categories.Find(c => c.NormalizedName.Equals("stone-fruits"))},
        //    };
        }
      

        public async Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {

            var filteredFruits = string.IsNullOrEmpty(categoryNormalizedName)
            ? _fruits
            : _fruits.Where(f => f.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase)).ToList();


            var pagedFruits = filteredFruits
                .Skip((pageNo - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var totalItems = filteredFruits.Count;
            var listModel = new ListModel<Fruit>
            {
                Items = pagedFruits,
                TotalCount = totalItems,
                CurrentPage = pageNo,
                PageSize = _pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / _pageSize)
            };

            return new ResponseData<ListModel<Fruit>> { Data = listModel, Successfull = true };
        }


        public Task<ResponseData<Fruit>> GetProductByIdAsync(int id)
        {
            return Task.FromResult<ResponseData<Fruit>>(null);
        }

        public Task UpdateProductAsync(int id, Fruit product, IFormFile? formFile)
        {

            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int id)
        {

            return Task.CompletedTask;
        }

        public Task<ResponseData<Fruit>> CreateProductAsync(Fruit product, IFormFile? formFile)
        {

            return Task.FromResult<ResponseData<Fruit>>(null);
        }
    }
}
