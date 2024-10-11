namespace Blog.Core.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserModel User { get; set; }
        public int UserId { get; set; }
        public PostModel Post { get; set; }
        public int PostId { get; set; }
    }
}
