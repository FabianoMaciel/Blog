using AutoMapper;
using Blog.Core.Models;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _accessor;

        public UserHandler(AppDbContext context, IMapper mapper, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings, IHttpContextAccessor accessor)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _accessor = accessor;
        }

        public async Task<IEnumerable<AuthorModel>> GetAll()
        {
            var entities = await _context.Authors.Include(a => a.User).ToListAsync();
            var models = entities.Select(a => _mapper.Map<AuthorModel>(a));

            return models;
        }

        public async Task<AuthorModel> Get(int id)
        {
            return _mapper.Map<AuthorModel>(await _context.Authors.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id));
        }

        public bool Exists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Authors.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
            _context.Users.Remove(entity.User);
            _context.Authors.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthorModel> Add(AuthorModel model)
        {
            var entity = _mapper.Map<Author>(model);
            entity.CreatedAt = DateTime.Now;
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<string> Register(UserInsertModel registerUser)
        {
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
                return string.Join('|', result.Errors.Select(a => a.Description));

            if (registerUser.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
            }

            var entity = new Author();
            entity.User = user;
            entity.CreatedAt = DateTime.Now;
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJWT(user.Email);
            }

            return string.Empty;
        }

        public async Task<string> Login(LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, true);

            if (result.Succeeded)
            {
                return await GenerateJWT(login.Username);
            }

            return string.Empty;
        }

        public async Task<string> GenerateJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.RefreshHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        public bool IsAdmin()
        {
            return _accessor.HttpContext?.User.IsInRole("admin") ?? false;
        }

        public async Task<bool> IsAllowedAsync(string authorId)
        {
            if (string.IsNullOrEmpty(authorId)) return false;
            string loggerUser = await GetUserIdAsync();
            return authorId.Equals(loggerUser) || IsAdmin();
        }

        public async Task<string> GetUserIdAsync()
        {
            if (!IsAuthenticated()) return string.Empty;

            //TO DO why this is returning null from the nameidentifier from the api
            var userName = _accessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName.Equals(userName));

            return user.Id ?? string.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }

    }
}
