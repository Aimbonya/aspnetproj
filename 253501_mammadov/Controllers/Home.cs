using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using _253501_mammadov.Models;

namespace _253501_mammadov.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            // Создаем список объектов ListDemo
            var demoList = new List<ListDemo>
            {
            new ListDemo { Id = 1, Name = "Элемент 1" },
            new ListDemo { Id = 2, Name = "Элемент 2" },
            new ListDemo { Id = 3, Name = "Элемент 3" }
            };

            // Преобразуем список в SelectList и передаем его через ViewData
            ViewData["DemoList"] = new SelectList(demoList, "Id", "Name");

            ViewData["TitleText"] = "Лабораторная работа №2";
            return View();
        }
    }
}
