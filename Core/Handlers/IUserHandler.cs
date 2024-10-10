using Blog.Core.Models;

namespace Core.Handlers
{
    public interface IUserHandler
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel> Add(UserModel model);
        Task<UserModel> Get(int id);
        Task<UserModel> Edit(UserModel model);
        bool Exists(int id);
        Task Delete(int id);
    }
}
