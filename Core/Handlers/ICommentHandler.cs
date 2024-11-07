using Blog.Core.Models;

namespace Core.Handlers
{
    public interface ICommentHandler
    {
        Task<IEnumerable<CommentModel>> GetAll();
        Task<CommentModel> Add(CommentModel model);
        Task<CommentInsertModel> Add(CommentInsertModel model, string userId);
        Task<CommentModel> Get(int id);

        Task<List<CommentModel>> GetCommentsByPost(int postId);
        Task<CommentModel> Edit(CommentModel model);
        bool Exists(int id);
        Task<bool> Delete(int id);
    }
}
