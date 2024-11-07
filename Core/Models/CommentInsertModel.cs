using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Models
{
    public class CommentInsertModel
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
