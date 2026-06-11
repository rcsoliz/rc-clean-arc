using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public record CreatePostCommand(string PostContent, int UserId, string? ImageUrl, List<int> CategoryId) : IRequest<PostDto>;
    
}
