using Blog.Core.Models;

namespace Core.Handlers
{
    public interface ICommentHandler
    {
        Task<IEnumerable<CommentModel>> GetAll();
        Task<CommentModel> Add(CommentModel model);
        Task<CommentModel> Get(int id);
        Task<CommentModel> Edit(CommentModel model);
        bool Exists(int id);
        Task Delete(int id);
    }
}
