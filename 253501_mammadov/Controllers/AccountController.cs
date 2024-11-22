using _253501_mammadov.Models;
using _253501_mammadov.Services.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;

namespace _253501_mammadov.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(HttpClient httpClient, ILogger<AccountController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Метод GET: Отправляет форму регистрации
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel user,
        [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await authService.RegisterUserAsync(user.Email,
                user.Password,
                user.Avatar);
                if (result.Result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }

                else return BadRequest(result.ErrorMessage);
            }
            return View(user);
        }

        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
        }

        // Метод для выхода
        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
