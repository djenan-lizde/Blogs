using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Posts.Contracts.Requests;
using Posts.Contracts.Responses;
using Posts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posts.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogPostRepository _repositoryBlogPost;

        public PostsController(
            ILogger<PostsController> logger,
            IBlogPostRepository repositoryBlogPost
            )
        {
            _logger = logger;
            _repositoryBlogPost = repositoryBlogPost;
        }


        [HttpGet("{slug}")]
        [Produces(typeof(BlogPost))]
        public IActionResult GetBlogPost([FromRoute] string slug)
        {
            try
            {
                var blog = _repositoryBlogPost.GetBySlug(slug);

                if (blog == null)
                    return NotFound("There is no blog with that slug");

                return Ok(blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<Blogs>))]
        public IActionResult GetBlogPosts([FromQuery] string tag)
        {
            try
            {
                var posts = _repositoryBlogPost.GetByTag(tag).ToList();

                if (posts.Count == 0)
                    return NotFound("There is no posts with that tag");

                var blogPosts = new Blogs
                {
                    BlogPosts = new List<BlogPost>()
                };

                foreach (var post in posts)
                {
                    blogPosts.BlogPosts.Add(_repositoryBlogPost.GetBySlug(post));
                }
                blogPosts.PostsCount = blogPosts.BlogPosts.Count;

                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Produces(typeof(BlogPost))]
        public IActionResult PostBlog([FromBody] CreateBlogPost blogPost)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Some of the properties are not valid");

                string slug = blogPost.Title.Replace(' ', '-').ToLower();

                var blog = _repositoryBlogPost.GetBySlug(slug);

                if (blog != null)
                    return BadRequest("Blog with that slug already exists"); 
                
                blog = _repositoryBlogPost.Insert(blogPost, slug);

                return Ok(_repositoryBlogPost.GetBySlug(blog.Slug));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPut("{slug}")]
        [Produces(typeof(BlogPost))]
        public IActionResult UpdateBlog([FromBody] UpdateBlogPost model, [FromRoute] string slug)
        {
            try
            {
                var blogInDb = _repositoryBlogPost.GetBySlug(slug);

                if (blogInDb == null)
                    return NotFound("There is no blog with that slug");

                var blog = new BlogPost
                {
                    CreatedAt = blogInDb.CreatedAt,
                    Body = string.IsNullOrWhiteSpace(model.Body) ? blogInDb.Body : model.Body,
                    Description = string.IsNullOrWhiteSpace(model.Description) ? blogInDb.Description : model.Description,
                    Title = string.IsNullOrWhiteSpace(model.Title) ? blogInDb.Title : model.Title
                };
                blog.Slug = blog.Title.Replace(' ', '-').ToLower();

                _repositoryBlogPost.Update(blog, slug);

                return Ok(_repositoryBlogPost.GetBySlug(blog.Slug));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpDelete("{slug}")]
        public IActionResult DeleteBlog([FromRoute] string slug)
        {
            try
            {
                _repositoryBlogPost.Delete(slug);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}
