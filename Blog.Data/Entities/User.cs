using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public IdentityUser IdentityUser { get; set; }

        public string IdentityUserId { get; set; }

        public string Occupation { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
