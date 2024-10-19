using mammadov.Domain.Entities;
using _253501_mammadov.API.Models;

namespace _253501_mammadov.API.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
