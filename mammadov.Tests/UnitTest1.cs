using _253501_mammadov.Controllers;
using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.ProductService;
using _253501_mammadov.Models;
using mammadov.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mammadov.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task Index_Returns404_WhenCategoryListFails()
        {
            // Arrange
            var categoryService = Substitute.For<ICategoryService>();
            var fruitService = Substitute.For<IFruitService>();

            categoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Error("Error"));

            var controller = new ProductController(categoryService, fruitService);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Index_Returns404_WhenProductListFails()
        {
            // Arrange
            var categoryService = Substitute.For<ICategoryService>();
            var fruitService = Substitute.For<IFruitService>();

            categoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Success(new List<Category>()));
            fruitService.GetProductListAsync(null, 1).Returns(ResponseData<ListModel<Fruit>>.Error("Error"));

            var controller = new ProductController(categoryService, fruitService);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Index_SetsCorrectViewData_WhenCategoryIsNull()
        {
            // Arrange
            var categoryService = Substitute.For<ICategoryService>();
            var fruitService = Substitute.For<IFruitService>();

            var categories = new List<Category>
            {
                new Category { Name = "Fruits", NormalizedName = "fruits" },
                new Category { Name = "Vegetables", NormalizedName = "vegetables" }
            };

            categoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Success(categories));
            fruitService.GetProductListAsync(null, 1)
                .Returns(ResponseData<ListModel<Fruit>>.Success(new ListModel<Fruit>()));

            var controller = new ProductController(categoryService, fruitService);

            var httpContext = new DefaultHttpContext(); // Создаем HTTP-контекст без заголовков, чтобы имитировать обычный запрос
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Все", result.ViewData["currentCategory"]);
            Assert.Equal(categories, result.ViewData["categories"]);
        }

        [Fact]
        public async Task Index_SetsCorrectViewData_WhenCategoryIsNotNull()
        {
            // Arrange
            var categoryService = Substitute.For<ICategoryService>();
            var fruitService = Substitute.For<IFruitService>();

            var categories = new List<Category>
            {
                new Category { Name = "Fruits", NormalizedName = "fruits" },
                new Category { Name = "Vegetables", NormalizedName = "vegetables" }
            };

            categoryService.GetCategoryListAsync()
                .Returns(ResponseData<List<Category>>.Success(categories));

            var products = new List<Fruit>
    {
        new Fruit { Name = "Apple" },
        new Fruit { Name = "Banana" }
    };

            fruitService.GetProductListAsync("fruits", 1)
                .Returns(ResponseData<ListModel<Fruit>>.Success(new ListModel<Fruit>
                {
                    Items = products
                }));

            var controller = new ProductController(categoryService, fruitService);

            var httpContext = new DefaultHttpContext(); // Создаем HTTP-контекст без заголовков, чтобы имитировать обычный запрос
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Index("fruits") as ViewResult;

            // Assert
            Assert.NotNull(result);

            // Проверяем модель
            var returnedModel = result.Model as ListModel<Fruit>;
            Assert.NotNull(returnedModel);
            Assert.Equal(products, returnedModel.Items);

            // Проверяем ViewData для категорий
            var actualCategories = result.ViewData["categories"] as List<Category>;
            Assert.NotNull(actualCategories);
            Assert.Collection(actualCategories,
                category =>
                {
                    Assert.Equal("Fruits", category.Name);
                    Assert.Equal("fruits", category.NormalizedName);
                },
                category =>
                {
                    Assert.Equal("Vegetables", category.Name);
                    Assert.Equal("vegetables", category.NormalizedName);
                });

            // Проверяем текущую категорию
            Assert.Equal("Fruits", result.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var categoryService = Substitute.For<ICategoryService>();
            var fruitService = Substitute.For<IFruitService>();

            var categories = new List<Category>
            {
                new Category { Name = "Fruits", NormalizedName = "fruits" }
            };

            var products = new List<Fruit>
            {
                new Fruit { Name = "Apple" },
                new Fruit { Name = "Banana" }
            };

            var listModel = new ListModel<Fruit>
            {
                Items = products,
                TotalCount = products.Count
            };

            categoryService.GetCategoryListAsync()
                .Returns(ResponseData<List<Category>>.Success(categories));

            fruitService.GetProductListAsync(null, 1)
                .Returns(ResponseData<ListModel<Fruit>>.Success(listModel));

            var controller = new ProductController(categoryService, fruitService);

            // Настройка контекста HTTP
            var httpContext = new DefaultHttpContext(); // Создаем HTTP-контекст без заголовков, чтобы имитировать обычный запрос
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Index() as ViewResult;

            // Проверяем, что возвращенная модель совпадает с ожидаемой
            var returnedModel = result.Model as ListModel<Fruit>;
            Assert.NotNull(returnedModel);
            Assert.Equal(products, returnedModel.Items); // Проверяем элементы модели

            // Проверяем ViewData
            Assert.Equal(categories, result.ViewData["categories"]); // Проверяем ViewData для категорий
            Assert.Equal("Все", result.ViewData["currentCategory"]); // Проверяем текущую категорию
        }
    }
}
