using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Post → PostDto
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.PostCategories.Select(c => new PostCategoryDtos
                {
                    CategoryId = c.Category.Id,
                    Name = c.Category.Name
                })));

            // PostCategory → PostCategoryDtos
            CreateMap<PostCategory, PostCategoryDtos>()
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));
                 
        }

    }
}
