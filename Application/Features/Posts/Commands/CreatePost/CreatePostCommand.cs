using Application.DTOs;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public record CreatePostCommand(string PostContent, int UserId) : IRequest<Post>;
    
}
