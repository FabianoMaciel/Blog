using AutoMapper;
using Blog.Core.Models;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers
{
    public class CommentHandler : ICommentHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CommentHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentModel>> GetAll()
        {
            var entities = await _context.Comments.Include(c => c.Post).Include(c => c.Author).ToListAsync();
            var models = entities.Select(a => _mapper.Map<CommentModel>(a));

            return models;
        }

        public async Task<CommentModel> Add(CommentModel model)
        {
            model.AuthorId = 1;
            var entity = _mapper.Map<Comment>(model);
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);
            await _context.SaveChangesAsync();

            model.Id = entity.Id;

            return model;
        }

        public async Task<CommentModel> Get(int id)
        {
            var entity = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<CommentModel>(entity);
        }

        public async Task<CommentModel> Edit(CommentModel model)
        {
            var entity = _mapper.Map<Comment>(model);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public bool Exists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
