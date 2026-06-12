using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Commands.CreatePostCategories
{
    public record CreatePostCategoryCommand(string PostContent, int UserId, string? ImageUrl, List<int> CategoryIds) : IRequest<PostDto>;
    
}
