using Microsoft.AspNetCore.Mvc;

namespace _253501_mammadov.Views.Shared.Components
{

    public class CartModel
    {
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var cartModel = new CartModel
            {
                TotalAmount = 0,
                ItemCount = 0
            };

            return View(cartModel);
        }
    }
}
