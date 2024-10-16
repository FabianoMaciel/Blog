using Blog.Core.Models;

namespace Core.Handlers
{
    public interface IPostHandler
    {
        Task<IEnumerable<PostModel>> GetAll(bool isFromApi = false);
        Task<PostModel> Add(PostModel model);
        Task<PostModel> Get(int id);
        Task<PostModel> Edit(PostModel model);
        bool Exists(int id);
        Task Delete(int id);
    }
}
