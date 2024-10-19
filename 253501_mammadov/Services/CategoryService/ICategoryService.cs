using _253501_mammadov.Models;
using mammadov.Domain.Entities;

namespace _253501_mammadov.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
