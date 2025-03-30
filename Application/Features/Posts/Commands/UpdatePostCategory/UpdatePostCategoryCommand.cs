using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.UpdatePostCategory
{
    public record UpdatePostCategoryCommand(int Id, string PostContent, int UserId, string ImageUrl, List<int> CategoryIds) : IRequest<(Post post, List<int> CategoryIds)>;
}
