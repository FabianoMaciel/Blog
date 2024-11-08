using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
