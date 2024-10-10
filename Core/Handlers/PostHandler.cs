using AutoMapper;
using Blog.Core.Models;
using Blog.Data;
using Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers
{
    public class PostHandler : IPostHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PostHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostModel>> GetAll()
        {
            var entities = await _context.Posts.Include(p => p.Autor).ToListAsync();
            var models = entities.Select(a => _mapper.Map<PostModel>(a));

            return models;
        }

        public async Task<PostModel> Add(PostModel model)
        {
            var entity = _mapper.Map<Post>(model);
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);
            await _context.SaveChangesAsync();

            model.Id = entity.Id;

            return model;
        }

        public async Task<PostModel> Get(int id)
        {
            var entity = await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<PostModel>(entity);
        }

        public async Task<PostModel> Edit(PostModel model)
        {
            var entity = _mapper.Map<Post>(model);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public bool Exists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
