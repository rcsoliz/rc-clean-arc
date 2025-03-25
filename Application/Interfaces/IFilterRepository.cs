using Application.Common;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IFilterRepository
    {
        Task<PagedResult<PostDto>> FiltersPost(PostFilterDto filter);

    }
}
