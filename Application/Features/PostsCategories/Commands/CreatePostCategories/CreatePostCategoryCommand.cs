using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.CreatePostCategories
{
    public record CreatePostCategoryCommand(string PostContent, string UserId, string ImageUrl, List<int> CategoryIds) : IRequest<(Post post, List<int> categories)>;
    
}
