using Application.Interfaces;
using Application.Queries.PostCommands;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.PostHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostContent = request.PostContent,
                UserId = Convert.ToInt32(request.UserId)

            };

            await _postRepository.AddAsync(post);
            return post;
        }
    }
}
