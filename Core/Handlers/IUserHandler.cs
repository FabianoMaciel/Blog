using Blog.Core.Models;
using Core.Models;

namespace Core.Handlers
{
    public interface IUserHandler
    {
        Task<IEnumerable<AuthorModel>> GetAll();
        Task<AuthorModel> Add(AuthorModel model);
        Task<AuthorModel> Get(string id);
        Task<AuthorModel> Edit(AuthorModel model);
        bool Exists(int id);
        Task Delete(int id);

        Task<string> Register(UserInsertModel registerUser);

        Task<string> Login(LoginModel login);
    }
}
