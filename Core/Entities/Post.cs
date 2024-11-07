using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public IdentityUser Author { get; set; }

        public string AuthorId { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
