using Application.Common;
using Core.Dtos;

namespace Application.Interfaces
{
    public interface IFilterRepository
    {
        Task<PagedResult<PostDto>> FiltersPost(PostFilterDto filter);

    }
}
