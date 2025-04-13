using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById
{
    public class GetPostByIdWithCategoriesHandler : IRequestHandler<GetPostByIdWithCategoriesQuery, PostWithCategoriesDto>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;
        public GetPostByIdWithCategoriesHandler(IPostCategoryRepository postCategoryRepository, IMapper mapper)
        {
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }
        public async Task<PostWithCategoriesDto> Handle(GetPostByIdWithCategoriesQuery request, CancellationToken cancellationToken)
        {
            var post = await _postCategoryRepository.GetPostByIdtWithCategoriesAsync(request.id);
            if (post == null) return null;

            return _mapper.Map<PostWithCategoriesDto>(post);
        }
    }
  
}
