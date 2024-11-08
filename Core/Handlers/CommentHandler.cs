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
        private readonly IUserHandler _userHandler;
        public CommentHandler(AppDbContext context, IMapper mapper, IUserHandler userHandler)
        {
            _context = context;
            _mapper = mapper;
            _userHandler = userHandler;
        }

        public async Task<IEnumerable<CommentModel>> GetAll()
        {
            var entities = await _context.Comments.Include(c => c.Post).Include(c => c.Author).ToListAsync();
            var models = entities.Select(a => _mapper.Map<CommentModel>(a));

            return models;
        }

        public async Task<CommentModel> Add(CommentModel model)
        {
            model.AuthorId = await _userHandler.GetUserIdAsync();
            var entity = _mapper.Map<Comment>(model);
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);
            await _context.SaveChangesAsync();

            model.Id = entity.Id;

            return model;
        }

        public async Task<CommentInsertModel> Add(CommentInsertModel model, string userId)
        {
            var entity = _mapper.Map<Comment>(model);
            entity.AuthorId = userId;
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);

            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<CommentModel> Get(int id)
        {
            var entity = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            var model = _mapper.Map<CommentModel>(entity);

            model.IsUserAllowedToEdit = await _userHandler.IsAllowedAsync(model.AuthorId);

            return model;
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

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Comments.FindAsync(id);
            if (await _userHandler.IsAllowedAsync(entity.AuthorId))
            {
                _context.Comments.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<CommentModel>> GetCommentsByPost(int postId)
        {
            var post = await _context.Posts.Include(a => a.Comments).FirstOrDefaultAsync(a => a.Id == postId);

            return post.Comments.Select(a => _mapper.Map<CommentModel>(a)).ToList();
        }
    }
}
