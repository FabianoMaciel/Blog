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
            // .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<Post, PostModel>();
            CreateMap<Comment, CommentModel>();
            CreateMap<UserModel, User>();
              //  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<PostModel, Post>();
            CreateMap<CommentModel, Comment>();
        }
    }
}
