using mammadov.Domain.Entities;
using _253501_mammadov.Models;
using System.Text.Json;
using System.Text;

namespace _253501_mammadov.Services.ProductService
{
    public class ApiProductService : IFruitService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Fruit>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}fruits/");
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }

            var response = await _httpClient.GetAsync(urlString.ToString());           

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Fruit>>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Error: {response.StatusCode}");
            return ResponseData<ListModel<Fruit>>.Error($"Error: {response.StatusCode}");
        }

        Task<ResponseData<Fruit>> IFruitService.CreateProductAsync(Fruit product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        Task IFruitService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Fruit>> IFruitService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IFruitService.UpdateProductAsync(int id, Fruit product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
