﻿using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public IdentityUser Author { get; set; }

        public int AuthorId { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
