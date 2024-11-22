namespace _253501_mammadov.Models
{
	public class ErrorViewModel
	{
		// Свойство для хранения уникального идентификатора ошибки
		public string? RequestId { get; set; }

		// Флаг, показывающий, следует ли отображать идентификатор ошибки
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
