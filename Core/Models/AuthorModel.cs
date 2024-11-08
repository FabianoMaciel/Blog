using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Models
{
    public class AuthorModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
