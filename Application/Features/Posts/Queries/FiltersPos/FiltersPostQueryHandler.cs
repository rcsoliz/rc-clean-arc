using Application.Common;
using Application.Interfaces;
using Core.Dtos;
using MediatR;


namespace Application.Features.Posts.Queries.FiltersPos
{
    public class FiltersPostQueryHandler : IRequestHandler<FiltersPostQuery, PagedResult<PostDto>>
    {
        private readonly IFilterRepository _filterRepository;

        public FiltersPostQueryHandler(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }
        public async Task<PagedResult<PostDto>> Handle(FiltersPostQuery request, CancellationToken cancellationToken)
        {
            return await _filterRepository.FiltersPost(request.filter);
        }
    }
}
