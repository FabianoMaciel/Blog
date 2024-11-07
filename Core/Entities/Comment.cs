using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public IdentityUser Author { get; set; }
        public string AuthorId { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
