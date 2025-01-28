using _253501_mammadov.Extensions;
using mammadov.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace _253501_mammadov.Views.Shared.Components
{
    public class CartViewComponent : ViewComponent
    {
        private const string CartSessionKey = "cart";

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetObject<Cart>(CartSessionKey) ?? new Cart();
            return View(cart);
        }
    }
}
