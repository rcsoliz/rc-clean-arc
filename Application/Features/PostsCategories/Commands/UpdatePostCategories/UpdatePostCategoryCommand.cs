using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.UpdatePostCategories
{
    public record UpdatePostCategoryCommand(int Id, string PostContent, int UserId, string ImageUrl, List<int> CategoryIds) : IRequest<(Post post, List<int> CategoryIds)>;
}
