using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Display(Name = "Comment: ")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public IdentityUser Author { get; set; }
        public string AuthorId { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
