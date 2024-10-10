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
    public interface IUserHandler
    {
        Task<IEnumerable<UserModel>> GetUsers();
    }
}
