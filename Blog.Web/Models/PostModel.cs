using System;

namespace Blog.Web.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserModel Autor { get; set; }

        public int AutorId { get; set; }
    }
}
