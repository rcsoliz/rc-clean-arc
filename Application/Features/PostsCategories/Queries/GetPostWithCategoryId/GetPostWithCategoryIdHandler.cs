using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetAllPostWithCategoryId
{
    public class GetPostWithCategoryIdHandler : IRequestHandler<GetPostWithCategoryIdQuery, IEnumerable<PostWithCategoriesDto>>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;
        public GetPostWithCategoryIdHandler(IPostCategoryRepository postCategoryRepository, IMapper mapper)
        {
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PostWithCategoriesDto>> Handle(GetPostWithCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postCategoryRepository.GetPostWithCategoryIdAsync(request.categoryId);
            if (posts == null) return null;

            return _mapper.Map<IEnumerable<PostWithCategoriesDto>>(posts);
        }

    }
    
}
