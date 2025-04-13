using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetAllPostsWithCategories
{
    public class GetAllPostsWithCategoriesHandler : IRequestHandler<GetAllPostsWithCategoriesQuery, IEnumerable<PostWithCategoriesDto>>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;

        public GetAllPostsWithCategoriesHandler(IPostCategoryRepository postCategoryRepository, IMapper mapper)
        {
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostWithCategoriesDto>> Handle(GetAllPostsWithCategoriesQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postCategoryRepository.GetAllPostsWithCategoriesAsync();
            if (posts == null) return null;

            return _mapper.Map<IEnumerable<PostWithCategoriesDto>>(posts);
        }
    }

}
