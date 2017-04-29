using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalifornia.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            //return new ContentResult {Content = "Blog posts"};
            return View();
        }

        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            //return new ContentResult
            //{
            //    Content = string.Format($"Year: {year};\nMonth: {month};\nKey: {key}")
            //};

            // ViewBag property: This property is a dynamic object that is accessible both on the controller and the view.

            ViewBag.Title = "My blog post";
            ViewBag.Posted = DateTime.Now;
            ViewBag.Author = "ystvan";
            ViewBag.Body = "This is a great post. innit?";


            return View();
        }
    }
}
