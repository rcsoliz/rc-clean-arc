using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public record CreatePostCommand(string PostContent, string UserId) : IRequest<Post>;
    
}
