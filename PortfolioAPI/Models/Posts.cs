namespace PortfolioAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Reply> Replies { get; set; } = new();
    }
}