using AutoMapper;
using Blog.Core.Models;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Handlers
{
    public class PostHandler : IPostHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHandler _userHandler;
        private readonly JwtSettings _jwtSettings;

        public PostHandler(AppDbContext context, IMapper mapper, IUserHandler userHandler, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _mapper = mapper;
            _userHandler = userHandler;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<PostModel>> GetAll(bool isFromApi)
        {
            List<Post> entities;
            if (!isFromApi)
                entities = await _context.Posts.Include(p => p.Author).Include(p => p.Comments).ToListAsync();
            else
                entities = await _context.Posts.Include(p => p.Author).ToListAsync();

            var models = entities.Select(a => _mapper.Map<PostModel>(a));

            return models;
        }

        public async Task<PostInsertModel> Add(PostInsertModel model, string loggedUser)
        {
            var entity = _mapper.Map<Post>(model);
            entity.CreatedAt = DateTime.Now;
            entity.AuthorId = loggedUser;

            _context.Add(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<PostModel> Get(int id)
        {
            var entity = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            var model = _mapper.Map<PostModel>(entity);

            model.IsAllowedToEdit = await _userHandler.IsAllowedAsync(entity.AuthorId);

            return model;
        }

        public async Task<PostInsertModel> Edit(int id, [FromBody] PostInsertModel model, string loggedUser)
        {
            var entity = await _context.Posts.FindAsync(id);
            if (entity == null)
                return null;

            if (await _userHandler.IsAllowedAsync(entity.AuthorId))
            {
                entity.Title = model.Title;
                entity.Content = model.Content;
                _context.Update(entity);
                await _context.SaveChangesAsync();

                return model;
            }

            return null;
        }

        public bool Exists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        public async Task<int> Delete(int id, string loggedUser)
        {
            var entity = await _context.Posts.FindAsync(id);

            if (await _userHandler.IsAllowedAsync(entity.AuthorId))
            {
                _context.Posts.Remove(entity);
                await _context.SaveChangesAsync();
                return StatusCodes.Status204NoContent;
            }

            return StatusCodes.Status403Forbidden;
        }
    }
}
