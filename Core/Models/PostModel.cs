using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Content is Required")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public IdentityUser Author { get; set; }

        public string AuthorId { get; set; }

        public List<Comment> Comments { get; set; }

        public bool IsAllowedToEdit { get; set; }
    }
}
