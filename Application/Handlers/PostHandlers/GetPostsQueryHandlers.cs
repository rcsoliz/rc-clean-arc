using Application.Interfaces;
using Application.Queries.PostCommands;
using Core.Entities;
using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.PostHandlers
{
    public class GetPostsQueryHandlers : IRequestHandler<GetAllPostQuery, IEnumerable<Post>>
    {
        private readonly IPostRepository _postRepository;

        public GetPostsQueryHandlers(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Post>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllAsync();
        }
    }
}
