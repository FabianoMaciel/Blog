using Blog.Core.Models;

namespace Core.Handlers
{
    public interface IPostHandler
    {
        Task<IEnumerable<PostModel>> GetAll(bool isFromApi = false);
        Task<PostInsertModel> Add(PostInsertModel model, string loggedUser);
        Task<PostModel> Get(int id);
        Task<PostInsertModel> Edit(int id, PostInsertModel model, string loggedUser);
        bool Exists(int id);
        Task<int> Delete(int id, string loggedUser);
    }
}
