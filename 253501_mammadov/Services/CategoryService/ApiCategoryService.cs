using _253501_mammadov.Models;
using mammadov.Domain.Entities;
using System.Text.Json;

namespace _253501_mammadov.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiCategoryService> _logger;

         public ApiCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiCategoryService> logger)
         {
             _httpClient = httpClient;
             _serializerOptions = new JsonSerializerOptions
             {
                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
             };
             _logger = logger;
         }

         public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
         {
             // URL для запроса категорий
              var urlString = $"{_httpClient.BaseAddress}categories/";
              try
              {
                    // Отправляем GET-запрос
                    var response = await _httpClient.GetAsync(urlString);
                    if (response.IsSuccessStatusCode)
                    {
                          // Десериализуем ответ в объект ResponseData<List<Category>>
                          var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                          return data;
                    }

                    // Логируем ошибку, если запрос не успешен
                    _logger.LogError($"Error: {response.StatusCode}");
                    return ResponseData<List<Category>>.Error($"Error: {response.StatusCode}");
              }
              catch (Exception ex)
              {
                   // Обработка исключений
                   _logger.LogError($"Exception: {ex.Message}");
                   return ResponseData<List<Category>>.Error("An error occurred while fetching the categories.");
              }
            
         }
    }
}
