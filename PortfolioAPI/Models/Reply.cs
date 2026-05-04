namespace PortfolioAPI.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}