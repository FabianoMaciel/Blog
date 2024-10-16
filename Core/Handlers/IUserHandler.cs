using Blog.Core.Models;
using Core.Models;

namespace Core.Handlers
{
    public interface IUserHandler
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel> Add(UserModel model);
        Task<UserModel> Get(string id);
        Task<UserModel> Edit(UserModel model);
        bool Exists(int id);
        Task Delete(int id);

        Task<string> Register(UserModel registerUser);

        Task<string> Login(LoginModel login);
    }
}
