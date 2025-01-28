namespace _253501_mammadov.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Проверяет, является ли запрос асинхронным.
        /// </summary>
        /// <param name="request">Объект запроса HttpRequest.</param>
        /// <returns>True, если запрос асинхронный, иначе False.</returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
