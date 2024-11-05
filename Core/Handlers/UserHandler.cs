using AutoMapper;
using Blog.Core.Models;
using Core.Entities;
using Core.Models;
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

        public UserHandler(AppDbContext context, IMapper mapper, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<AuthorModel>> GetAll()
        {
            var entities = await _context.Authors.ToListAsync();
            var models = entities.Select(a => _mapper.Map<AuthorModel>(a));

            return models;
        }

        public async Task<AuthorModel> Get(string id)
        {
            return _mapper.Map<AuthorModel>(await _context.Users.FindAsync(id));
        }

        public async Task<AuthorModel> Edit(AuthorModel model)
        {
            var entity = _mapper.Map<Author>(model);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return model;
        }

        public bool Exists(int id)
        {
            return _context.Users.Any(e => e.Id.Equals(id));
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            _context.Users.Remove(entity);
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
            if (registerUser.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
            }

            var entity = _mapper.Map<Author>(registerUser);
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
    }
}
