using AutoMapper;
using Blog.Core.Models;
using Blog.Data;
using Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var entities = await _context.Users.ToListAsync();
            var models = entities.Select(a => _mapper.Map<UserModel>(a));

            return models;
        }

        public async Task<UserModel> Add(UserModel model)
        {
            var entity = _mapper.Map<User>(model);
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);
            await _context.SaveChangesAsync();

            model.Id = entity.Id;
            return model;
        }

        public async Task<UserModel> Get(int id)
        {
            return _mapper.Map<UserModel>(await _context.Users.FindAsync(id));
        }

        public async Task<UserModel> Edit(UserModel model)
        {
            var entity = _mapper.Map<User>(model);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public bool Exists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
