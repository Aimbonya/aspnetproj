using _253501_mammadov.API.Data;
using _253501_mammadov.API.Models;
using mammadov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _253501_mammadov.API.Services
{
    public class FruitService : IFruitService
    {
        private readonly int _maxPageSize = 20;

        private readonly AppDbContext _context;

        // Инжектируем контекст базы данных через конструктор
        public FruitService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<Fruit>> CreateProductAsync(Fruit product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Fruit>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Fruits.AsQueryable();

            var dataList = new ListModel<Fruit>();
            query = query
            .Where(d => categoryNormalizedName == null
                                    ||
            d.Category.NormalizedName.Equals(categoryNormalizedName));

            // количество элементов в списке
            var count = await query.CountAsync(); //.Count();
            if (count == 0)
            {
                return ResponseData<ListModel<Fruit>>.Success(dataList);
            }

            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Fruit>>.Error("No such page");

            dataList.Items = await query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ListModel<Fruit>>.Success(dataList);
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Fruit product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
