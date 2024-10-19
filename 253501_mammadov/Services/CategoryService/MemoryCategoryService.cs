using _253501_mammadov.Models;
using mammadov.Domain.Entities;

namespace _253501_mammadov.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Цитрусовые", NormalizedName="citrus-fruits"},
                new Category {Id=2, Name="Ягодные", NormalizedName="berries"},
                new Category {Id=3, Name="Косточковые", NormalizedName="stone-fruits"},
                new Category {Id=4, Name="Экзотические", NormalizedName="exotic-fruits"}
            };
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
