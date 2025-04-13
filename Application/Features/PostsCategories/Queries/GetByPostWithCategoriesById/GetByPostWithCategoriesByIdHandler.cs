using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById
{
    public class GetByPostWithCategoriesByIdHandler : IRequestHandler<GetByPostWithCategoriesByIdQuery, IEnumerable<PostWithCategoriesDto>>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        public GetByPostWithCategoriesByIdHandler(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }
        public async Task<IEnumerable<PostWithCategoriesDto>> Handle(GetByPostWithCategoriesByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postCategoryRepository.GetByPostWithCategoriesByIdAsync(request.id);
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
