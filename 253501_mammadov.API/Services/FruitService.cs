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

        public FruitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<Fruit>> CreateProductAsync(Fruit product, IFormFile? formFile)
        {
            if (product == null)
            {
                return ResponseData<Fruit>.Error("Product cannot be null");
            }

            // Обработка файла изображения, если он передан
            if (formFile != null)
            {
                var imageUrl = await SaveImageAsync(formFile); // Реализация SaveImageAsync ниже
                if (imageUrl == null)
                {
                    return ResponseData<Fruit>.Error("Failed to save image");
                }
                product.Image = imageUrl.Data;
            }

            // Добавляем продукт в базу данных
            _context.Fruits.Add(product);

            // Сохраняем изменения
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseData<Fruit>.Error($"Failed to create product: {ex.Message}");
            }

            return ResponseData<Fruit>.Success(product);
        }

        public async Task<ResponseData<bool>> DeleteProductAsync(int id)
        {
            // Ищем продукт в базе данных по ID
            var product = await _context.Fruits.FindAsync(id);

            if (product == null)
            {
                return ResponseData<bool>.Error($"Product with ID {id} not found");
            }

            // Удаляем продукт
            _context.Fruits.Remove(product);

            try
            {
                // Сохраняем изменения
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Error($"Failed to delete product: {ex.Message}");
            }

            return ResponseData<bool>.Success(true);
        }

        public async Task<ResponseData<Fruit>> GetProductByIdAsync(int id)
        {
            // Ищем продукт в базе данных по ID
            var product = await _context.Fruits
                .FirstOrDefaultAsync(f => f.Id == id);

            if (product == null)
            {
                return ResponseData<Fruit>.Error($"Product with ID {id} not found");
            }

            return ResponseData<Fruit>.Success(product);
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

        public async Task<ResponseData<string?>> SaveImageAsync(IFormFile formFile)
        {
            try
            {
                var folderPath = Path.Combine("wwwroot", "images", "products");
                Directory.CreateDirectory(folderPath); // Создаем папку, если ее нет

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                // Возвращаем относительный путь
                return ResponseData<string>.Success(new string(Path.Combine("images", "products", fileName).Replace("\\", "/")));
            }
            catch
            {
                return ResponseData<string>.Error("path not founf maybe idk"); // Возвращаем null в случае ошибки
            }
        }


        public async Task<ResponseData<Fruit>> UpdateProductAsync(int id, Fruit product)
        {
            var existingProduct = await _context.Fruits.FindAsync(id);
            if (existingProduct == null)
            {
                return ResponseData<Fruit>.Error("Product not found");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image;
            existingProduct.CategoryId = product.CategoryId;

            _context.Fruits.Update(existingProduct);
            await _context.SaveChangesAsync();

            return ResponseData<Fruit>.Success(existingProduct);
        }

    }
}
