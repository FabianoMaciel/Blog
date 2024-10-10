using Blog.Core.Models;

namespace Core.Handlers
{
    public interface IPostHandler
    {
        Task<IEnumerable<PostModel>> GetAll();
        Task<PostModel> Add(PostModel model);
        Task<PostModel> Get(int id);
        Task<PostModel> Edit(PostModel model);
        bool Exists(int id);
        Task Delete(int id);
    }
}
