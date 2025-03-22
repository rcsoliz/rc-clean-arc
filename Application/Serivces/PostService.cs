using Core.Dtos;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Serivces
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync()
        {
            return await _postRepository.GetAllPostWithDetailsAsync();
        }
    }
}
