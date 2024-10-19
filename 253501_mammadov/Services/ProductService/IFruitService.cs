using _253501_mammadov.Models;
using mammadov.Domain.Entities;

namespace _253501_mammadov.Services.ProductService
{
    public interface IFruitService
    {
        public Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        public Task<ResponseData<Fruit>> GetProductByIdAsync(int id);

        public Task UpdateProductAsync(int id, Fruit product, IFormFile? formFile);

        public Task DeleteProductAsync(int id);

        public Task<ResponseData<Fruit>> CreateProductAsync(Fruit product, IFormFile? formFile);
    }
}
