using _253501_mammadov;
using _253501_mammadov.API.Data;
using _253501_mammadov.API.Services;
using mammadov.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace mammadov.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();


            var categories = new List<Category>
                    {
                        new Category { Name="Цитрусовые", NormalizedName="citrus-fruits" },
                        new Category { Name="Ягодные", NormalizedName="berries" },
                        new Category { Name="Косточковые", NormalizedName="stone-fruits" },
                        new Category { Name="Экзотические", NormalizedName="exotic-fruits" }
                    };


            var fruits = new List<Fruit>
                    {
                        new Fruit { Name = "Апельсин", Description = "Сочный цитрусовый фрукт", Price = 120, Image = "Images/orange.jpg", ImageMimeType = "image/jpeg", Category = categories[0] },
                        new Fruit { Name = "Лимон", Description = "Кислый цитрусовый фрукт", Price = 100, Image = "Images/lemon.jpg", ImageMimeType = "image/jpeg", Category = categories[0] },
                        new Fruit { Name = "Клубника", Description = "Сочная ягода", Price = 150, Image = "Images/strawberry.jpg", ImageMimeType = "image/jpeg", Category = categories[1] },
                        new Fruit { Name = "Малина", Description = "Нежная ягода", Price = 200, Image = "Images/raspberry.jpg", ImageMimeType = "image/jpeg", Category = categories[1] },
                        new Fruit { Name = "Персик", Description = "Сладкий косточковый фрукт", Price = 180, Image = "Images/peach.jpg", ImageMimeType = "image/jpeg", Category = categories[2] },
                        new Fruit { Name = "Абрикос", Description = "Кисло-сладкий косточковый фрукт", Price = 170, Image = "Images/apricot.jpg", ImageMimeType = "image/jpeg", Category = categories[2] },
                        new Fruit { Name = "Манго", Description = "Экзотический сладкий фрукт", Price = 250, Image = "Images/mango.jpg", ImageMimeType = "image/jpeg", Category = categories[3] },
                        new Fruit { Name = "Ананас", Description = "Тропический фрукт", Price = 300, Image = "Images/pineapple.jpg", ImageMimeType = "image/jpeg", Category = categories[3]  }
                    };

            // Добавляем тестовые данные
            context.Fruits.AddRange(fruits);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new FruitService(context); 

            var result = await service.GetProductListAsync(null);

            Assert.IsType<_253501_mammadov.API.Models.ResponseData< _253501_mammadov.API.Models.ListModel <Fruit>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(3, result.Data.TotalPages); // 5 товаров, 2 страницы по 3
            Assert.Equal("Апельсин", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceReturnsSecondPageOfItems()
        {
            using var context = CreateContext();
            var service = new FruitService(context);

            var result = await service.GetProductListAsync(null, pageNo: 2);

            Assert.True(result.Successfull);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count); // Осталось 2 товара на второй странице
            Assert.Equal("Малина", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceFiltersItemsByCategory()
        {
            using var context = CreateContext();
            var service = new FruitService(context);

            var result = await service.GetProductListAsync("berries");

            Assert.True(result.Successfull);
            Assert.Equal(2 , result.Data.Items.Count);
            Assert.All(result.Data.Items, p => Assert.Equal(2, p.CategoryId));
        }

        [Fact]
        public async Task ServiceRestrictsPageSizeToMaximum()
        {
            using var context = CreateContext();
            var service = new FruitService(context);

            // Задаём слишком большой размер страницы
            var result = await service.GetProductListAsync(null, pageSize: 100);

            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.PageSize); 
            Assert.Equal(8, result.Data.Items.Count);
        }

        [Fact]
        public async Task ServiceReturnsErrorWhenPageNumberExceedsTotalPages()
        {
            using var context = CreateContext();
            var service = new FruitService(context);

            var result = await service.GetProductListAsync(null, pageNo: 10);

            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
