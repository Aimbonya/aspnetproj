using mammadov.Domain.Entities;
using _253501_mammadov.Models;
using System.Text.Json;
using System.Text;
using _253501_mammadov.Services.FileService;
using _253501_mammadov.Services.CategoryService;
using _253501_mammadov.Services.Authentication;

namespace _253501_mammadov.Services.ProductService
{
    public class ApiProductService : IFruitService
    {
        private readonly ICategoryService _categoryService;
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiProductService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger,
            IFileService fileService,
            ICategoryService categoryService,
            ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _fileService = fileService;
            _logger = logger;
            _categoryService = categoryService;
            _tokenAccessor = tokenAccessor;
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        }

        public async Task<ResponseData<Fruit>> GetProductByIdAsync(int id)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"fruits/{id}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Fruit>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации: {ex.Message}");
                    return ResponseData<Fruit>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка получения данных продукта: {response.StatusCode}");
            return ResponseData<Fruit>.Error($"Ошибка: {response.StatusCode}");
        }

        public async Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            await AddAuthorizationHeaderAsync();
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}fruits");

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"/{categoryNormalizedName}");
            }
            urlString.Append($"?pageNo={pageNo}&pageSize={_pageSize}");

            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Fruit>>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Error: {response.StatusCode}");
            return ResponseData<ListModel<Fruit>>.Error($"Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Fruit>> CreateProductAsync(Fruit product)
        {
            await AddAuthorizationHeaderAsync();
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "products");

            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Fruit>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"-----> object not created. Error: {response.StatusCode}");
            return ResponseData<Fruit>.Error($"Объект не добавлен. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Fruit>> UpdateProductAsync(int id, Fruit product, IFormFile? formFile)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"fruits/{id}", product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Fruit>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации при обновлении продукта: {ex.Message}");
                    return ResponseData<Fruit>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка обновления продукта: {response.StatusCode}");
            return ResponseData<Fruit>.Error($"Ошибка: {response.StatusCode}");
        }

        public async Task<ResponseData<Fruit>> DeleteProductAsync(int id)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"fruits/{id}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Fruit>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации при удалении продукта: {ex.Message}");
                    return ResponseData<Fruit>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка удаления продукта: {response.StatusCode}");
            return ResponseData<Fruit>.Error($"Ошибка: {response.StatusCode}");
        }
    }
}
