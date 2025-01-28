using Serilog;

namespace _253501_mammadov.MiddleWare
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Делаем запрос и получаем ответ

            await _next(context);

            var responseBody = context.Response.Body;

            try
            {
                // Создаем память для ответа, чтобы потом считать его содержимое
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    // Пропускаем запрос дальше по пайплайну
                    

                    // Возвращаемся и записываем в лог данные о запросе и ответе
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var responseText = new System.IO.StreamReader(memoryStream).ReadToEnd();

                    // Логируем только успешный ответ или в случае ошибки
                    if (context.Response.StatusCode >= 400)
                    {
                        Log.Error("Request {Method} {Url} failed with status code {StatusCode}. Response: {Response}",
                            context.Request.Method,
                            context.Request.Path,
                            context.Response.StatusCode,
                            responseText);
                    }
                    else
                    {
                        Log.Information("Request {Method} {Url} responded with status code {StatusCode}. Response: {Response}",
                            context.Request.Method,
                            context.Request.Path,
                            context.Response.StatusCode,
                            responseText);
                    }

                    // Записываем в исходный поток (например, в клиентский ответ)
                    await memoryStream.CopyToAsync(responseBody);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                throw; // Передаем ошибку дальше
            }
        }
    }
}
