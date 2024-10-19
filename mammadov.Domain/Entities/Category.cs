using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mammadov.Domain.Entities
{
    public class Category
    {
        // Уникальный идентификатор категории
        public int Id { get; set; }

        // Название категории
        public string Name { get; set; }

        // Нормализованное имя категории (для использования, например, в URL или базе данных)
        public string NormalizedName { get; set; }

        // Коллекция объектов Fruit, относящихся к этой категории (один ко многим)
        public ICollection<Fruit> Fruits { get; set; }

        public Category()
        {
            Fruits = new List<Fruit>();
        }
    }
}
