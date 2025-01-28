using System.ComponentModel.DataAnnotations;

public class CounterModel
{
	[Range(1, 10, ErrorMessage = "Введите значение от 1 до 10.")]
	public int? Value { get; set; }
}