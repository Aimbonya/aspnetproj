namespace mammadov.Domain.Entities
{
    public class Fruit
    {
        // Уникальный идентификатор фрукта
        public int Id { get; set; }

        // Название фрукта
        public string Name { get; set; }

        // Дополнительное описание
        public string Description { get; set; }

        // Цена фрукта
        public float Price { get; set; }

        // Путь к изображению
        public string Image { get; set; }

        // Mime-тип изображения
        public string ImageMimeType { get; set; }

        // Внешний ключ для связи с категорией
        public int CategoryId { get; set; }

        // Навигационное свойство для категории
        public Category? Category { get; set; }
    }
}
