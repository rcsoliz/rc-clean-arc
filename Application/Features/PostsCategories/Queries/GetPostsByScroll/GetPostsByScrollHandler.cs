using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetPostsByScroll
{
    public class GetPostsByScrollHandler: IRequestHandler<GetPostsByScrollQuery, IEnumerable<PostWithCategoriesDto>>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;
        public GetPostsByScrollHandler(IPostCategoryRepository postCategoryRepository, IMapper mapper)
        {
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PostWithCategoriesDto>> Handle(GetPostsByScrollQuery request, CancellationToken cancellationToken)
        {
            var posts =  await _postCategoryRepository.GetPostsByScrollAsync(request.LastPostDate, request.Take);
            return _mapper.Map<IEnumerable<PostWithCategoriesDto>>(posts);
        }
    }

}
