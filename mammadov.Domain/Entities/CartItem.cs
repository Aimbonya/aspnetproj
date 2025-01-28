using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mammadov.Domain.Entities
{
    public class CartItem
    {
        public Fruit? Item { get; set; } // Ваш объект (например, блюдо)
        public int Count { get; set; } // Количество
    }
}
