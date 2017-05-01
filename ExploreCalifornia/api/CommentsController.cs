using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalifornia.api
{
    [Route("api/posts/{postKey}/comments")]
    public class CommentsController : Controller
    {
        private readonly BlogDataContext _db;

        public CommentsController(BlogDataContext db)
        {
            _db = db;
        }

        
        [HttpGet]
        public IQueryable<Comment> Get(string postKey)
        {
            return _db.Comments.Where(x => x.Post.Key == postKey);
        }

        
        [HttpGet("{id}")]
        public Comment Get(long id)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
            return comment;
        }

        
        [HttpPost]
        public Comment Post(string postKey, [FromBody]Comment comment)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Key == postKey);

            if (post == null)
                return null;

            comment.Post = post;
            comment.Posted = DateTime.Now;
            comment.Author = User.Identity.Name;

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return comment;
        }

        
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Comment updated)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Body = updated.Body;

            _db.SaveChanges();

            return Ok();

        }

        
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);

            if (comment != null)
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
            }
        }
    }
}
