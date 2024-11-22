using _253501_mammadov.API .Models;
using mammadov.Domain.Entities;

namespace _253501_mammadov.API.Services
   
{
    public interface IFruitService
    {
        public Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(string? categoryNormalizedName, 
                                                                        int pageNo = 1,
                                                                        int pagesize=3);

        public Task<ResponseData<Fruit>> GetProductByIdAsync(int id);

        public Task<ResponseData<Fruit>> UpdateProductAsync(int id, Fruit product);

        public Task<ResponseData<bool>> DeleteProductAsync(int id);

        public Task<ResponseData<Fruit>> CreateProductAsync(Fruit product, IFormFile? formFile);

        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string?>> SaveImageAsync(IFormFile formFile);
    }
}
