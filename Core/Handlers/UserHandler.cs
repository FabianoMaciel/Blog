using AutoMapper;
using Blog.Core.Models;
using Blog.Data;
using Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var entities = await _context.Users.ToListAsync();
            var models = entities.Select(a => _mapper.Map<UserModel>(a));

            return models;
        }



    }
}
