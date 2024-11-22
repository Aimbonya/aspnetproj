using _253501_mammadov.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace _253501_mammadov.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        // Внедрение ITokenAccessor в конструктор
        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        // Метод для установки заголовка авторизации
        private async Task SetAuthorizationHeaderAsync()
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        }

        public async Task DeleteFileAsync(string fileUri)
        {
            if (string.IsNullOrWhiteSpace(fileUri))
                throw new ArgumentException("Invalid file URI");

            // Устанавливаем заголовок авторизации
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(fileUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to delete file. Status code: {response.StatusCode}");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Устанавливаем заголовок авторизации
            await SetAuthorizationHeaderAsync();

            // Создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            // Сформировать случайное имя файла, сохранив расширение
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            // Создать контент, содержащий поток загруженного файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            // Поместить контент в запрос
            request.Content = content;

            // Отправить запрос к API
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }
    }

}
