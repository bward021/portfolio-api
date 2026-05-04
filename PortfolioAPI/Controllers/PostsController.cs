using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts
                .Include(p => p.Replies)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // POST: api/posts
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(CreatePostDto dto)
        {
            var post = new Post
            {
                Username = dto.Username,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }

        // POST: api/posts/{id}/replies
        [HttpPost("{id}/replies")]
        public async Task<ActionResult<Reply>> CreateReply(int id, CreateReplyDto dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            var reply = new Reply
            {
                PostId = id,
                Username = dto.Username,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPosts), new { id = reply.Id }, reply);
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    public class CreateReplyDto
    {
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
    public class CreatePostDto
    {
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}