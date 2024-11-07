using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public IdentityUser Author { get; set; }
        public string AuthorId { get; set; }
        public PostModel Post { get; set; }
        public int PostId { get; set; }
    }
}
