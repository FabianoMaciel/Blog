using Blog.Data.Entities;
using System;

namespace Blog.Core.Models
{
    public class PostInsertModel
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
