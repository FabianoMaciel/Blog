using AutoMapper;
using Blog.Core.Models;
using Core.Entities;

namespace Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorModel>();
            CreateMap<Post, PostModel>();
            CreateMap<Comment, CommentModel>();
            CreateMap<AuthorModel, Author>();
            CreateMap<PostInsertModel, Post>();
            CreateMap<PostModel, Post>();
            CreateMap<CommentModel, Comment>();
        }
    }
}
