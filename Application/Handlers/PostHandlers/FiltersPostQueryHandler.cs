using Application.Common;
using Application.Interfaces;
using Application.Queries.PostCommands;
using Core.Dtos;
using MediatR;


namespace Application.Handlers.PostHandlers
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
