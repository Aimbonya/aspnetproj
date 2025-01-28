using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace _253501_mammadov.TagHelpers
{
    public class TimerTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Console.WriteLine("TimerTagHelper is called");

            output.TagName = "div";    // заменяет тег <timer> тегом <div>
                                       // устанавливаем содержимое элемента
            output.Content.SetContent($"Текущее время: {DateTime.Now.ToString("HH:mm:ss")}");
        }
    }
}
