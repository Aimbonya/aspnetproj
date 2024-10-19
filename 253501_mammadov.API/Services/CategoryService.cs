using _253501_mammadov.API.Data;
using mammadov.Domain.Entities;
using _253501_mammadov.API.Models;
using Microsoft.EntityFrameworkCore;

namespace _253501_mammadov.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
