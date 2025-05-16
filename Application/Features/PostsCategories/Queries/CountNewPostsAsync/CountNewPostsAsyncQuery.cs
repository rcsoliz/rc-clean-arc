using MediatR;

namespace Application.Features.PostsCategories.Queries.CountNewPostsAsync
{
    public record CountNewPostsAsyncQuery(DateTime? afterDate) : IRequest<int>;
 
}
