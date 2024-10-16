using AutoMapper;
using Blog.Core.Models;
using Blog.Data;
using Blog.Data.Entities;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;

namespace Core.Handlers
{
    public class PostHandler : IPostHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public PostHandler(AppDbContext context, IMapper mapper, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<PostModel>> GetAll(bool isFromApi)
        {
            List<Post> entities;
            if (!isFromApi)
                entities = await _context.Posts.Include(p => p.Autor).Include(p => p.Comments).ToListAsync();
            else
                entities = await _context.Posts.Include(p => p.Autor).ToListAsync();

            var models = entities.Select(a => _mapper.Map<PostModel>(a));

            return models;
        }

        public async Task<PostInsertModel> Add(PostInsertModel model, string loggedUser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loggedUser);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.IdentityUserId == identityUser.Id);

            var entity = _mapper.Map<Post>(model);
            entity.CreatedAt = DateTime.Now;
            entity.AutorId = user.Id;

            _context.Add(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<PostModel> Get(int id)
        {
            var entity = await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<PostModel>(entity);
        }

        public async Task<PostInsertModel> Edit(int id, [FromBody] PostInsertModel model, string loggedUser)
        {
            var entity = await _context.Posts.FindAsync(id);
            if (entity == null)
                return null;

            bool allowed = await IsAllowed(loggedUser, entity);
            if (allowed)
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
            bool allowed = await IsAllowed(loggedUser, entity);

            if (allowed)
            {
                _context.Posts.Remove(entity);
                await _context.SaveChangesAsync();
                return StatusCodes.Status204NoContent;
            }

            return StatusCodes.Status403Forbidden;
        }

        private async Task<bool> IsAllowed(string loggedUser, Post entity)
        {
            var identityUser = await _userManager.FindByEmailAsync(loggedUser);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.IdentityUserId == identityUser.Id);
            bool allowed = _signInManager.Context.User.IsInRole("admin") || entity.AutorId == user.Id;

            return allowed;
        }
    }
}
