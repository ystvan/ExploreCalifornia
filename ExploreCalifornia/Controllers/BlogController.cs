using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalifornia.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly BlogDataContext _db;

        public BlogController(BlogDataContext db)
        {
            _db = db;
        }


        /*
        [Route("")]
        public IActionResult Index()
        {
            //var posts = new[]
            //{
            //    new Post
            //    {
            //        Title = "My brand new blog post",
            //        Posted = DateTime.Now,
            //        Author = "ystvan",
            //        Body = "Another awesome story"
            //    },
            //    new Post
            //    {
            //        Title = "My second blog post",
            //        Posted = DateTime.Now,
            //        Author = "ystvan",
            //        Body = "Another, non stopping awesome story"
            //    },
            //    new Post
            //    {
            //        Title = "My third blog post",
            //        Posted = DateTime.Now,
            //        Author = "ystvan",
            //        Body = "The last one awesome story"
            //    }
            //};

            var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();

            //return new ContentResult {Content = "Blog posts"};
            return View(posts);
        }
        */

        [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pageSize = 2;
            var totalPosts = _db.Posts.Count();
            var totalPages = totalPosts / pageSize;
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToArray();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(posts);
            }

            return View(posts);
        }


        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            //return new ContentResult
            //{
            //    Content = string.Format($"Year: {year};\nMonth: {month};\nKey: {key}")
            //};

            // ViewBag property: This property is a dynamic object that is accessible both on the controller and the view.

            //ViewBag.Title = "My blog post";
            //ViewBag.Posted = DateTime.Now;
            //ViewBag.Author = "ystvan";
            //ViewBag.Body = "This is a great post. innit?";

            //var post = new Post
            //{
            //    Title = "My blog post",
            //    Posted = DateTime.Now,
            //    Author = "ystvan",
            //    Body = "Another awesome story"
            //};

            var post = _db.Posts.FirstOrDefault(x => x.Key == key);

            //passing it as a parameter
            return View(post);
        }

        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;

            //USING the local db
            _db.Posts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Post", "Blog", new {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }

    }
}
