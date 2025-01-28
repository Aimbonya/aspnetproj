using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mammadov.Domain.Entities
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине (ключ - идентификатор объекта)
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        public virtual void AddToCart(Fruit dish)
        {
            if (CartItems.ContainsKey(dish.Id))
            {
                CartItems[dish.Id].Count++;
            }
            else
            {
                CartItems[dish.Id] = new CartItem { Item = dish, Count = 1 };
            }
        }

        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        public virtual void RemoveItem(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count => CartItems.Sum(item => item.Value.Count);

        /// <summary>
        /// Общая калорийность
        /// </summary>
        public double TotalPrice =>
            CartItems.Sum(item => item.Value.Item.Price * item.Value.Count);
    }
}
