using mammadov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _253501_mammadov.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            var url = app.Configuration;
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();

           
            if (!context.Fruits.Any() && !context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        new Category { Name="Цитрусовые", NormalizedName="citrus-fruits" },
                        new Category { Name="Ягодные", NormalizedName="berries" },
                        new Category { Name="Косточковые", NormalizedName="stone-fruits" },
                        new Category { Name="Экзотические", NormalizedName="exotic-fruits" }
                    };
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();

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
                  
                    await context.Fruits.AddRangeAsync(fruits);
                    await context.SaveChangesAsync();
            }
        
            
        }
    }
}
