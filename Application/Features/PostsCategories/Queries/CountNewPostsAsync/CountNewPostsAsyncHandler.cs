using Application.Interfaces;
using MediatR;

namespace Application.Features.PostsCategories.Queries.CountNewPostsAsync
{
    public class CountNewPostsAsyncHandler : IRequestHandler<CountNewPostsAsyncQuery, int>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        public CountNewPostsAsyncHandler(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }
        public async Task<int> Handle(CountNewPostsAsyncQuery request, CancellationToken cancellationToken)
        {
            if (request.afterDate == null)
            {
                return 0;
            }
            return await _postCategoryRepository.CountNewPostsAsync(request.afterDate.Value);
        }
    }
  
}
