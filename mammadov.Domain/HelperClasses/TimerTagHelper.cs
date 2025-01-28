using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mammadov.Domain.HelperClasses
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
