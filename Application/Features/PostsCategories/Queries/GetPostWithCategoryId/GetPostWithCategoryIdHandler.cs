using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetAllPostWithCategoryId
{
    public class GetPostWithCategoryIdHandler : IRequestHandler<GetPostWithCategoryIdQuery, PostWithCategoriesDto>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        public GetPostWithCategoryIdHandler(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }
        public async Task<PostWithCategoriesDto> Handle(GetPostWithCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postCategoryRepository.GetPostWithCategoryIdAsync(request.categoryId);
            if (post == null) return null;

            var result = post
                .Select(p => new PostWithCategoriesDto
                { 
                    Post = p,
                    Categories = p.Categories
                }).ToList();

            return result;
        }

    }
    
}
