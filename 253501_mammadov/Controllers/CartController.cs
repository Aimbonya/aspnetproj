using mammadov.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using _253501_mammadov.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using _253501_mammadov.Services.ProductService;

namespace _253501_mammadov.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly Cart _cart;
        private readonly IFruitService _productService;


        public CartController(IFruitService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }


        [HttpPost]
        [Route("Add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);

            if (data.Successful)
            {
                _cart.AddToCart(data.Data);
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("Cart", _cart);
        }

        [HttpPost]
        [Route("Clear")]
        public IActionResult ClearCart()
        {
            _cart.ClearAll();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {

            _cart.RemoveItem(id);
            return RedirectToAction("Index", "Cart");
        }


        public IActionResult ToCart()
        {
            return Redirect("/Cart/Index");
        }

      
    }
}
