using AutoMapper;
using Blog.Core.Models;
using Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Post, PostModel>();
            CreateMap<Comment, CommentModel>();
            CreateMap<UserModel, User>();
            CreateMap<PostInsertModel, Post>();
            CreateMap<PostModel, Post>();
            CreateMap<CommentModel, Comment>();
        }
    }
}
