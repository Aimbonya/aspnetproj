using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _253501_mammadov.Models
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public bool Successful { get; set; } = true;


        public string? ErrorMessage { get; set; }

        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T> { Data = data };
        }

        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Successful = false,
                Data = data
            };
        }
    }
}