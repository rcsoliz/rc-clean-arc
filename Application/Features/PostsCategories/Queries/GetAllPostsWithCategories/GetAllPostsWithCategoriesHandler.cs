using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PostsCategories.Queries.GetAllPostsWithCategories
{
    public class GetAllPostsWithCategoriesHandler : IRequestHandler<GetAllPostsWithCategoriesQuery, IEnumerable<PostWithCategoriesDto>>
    {
        private readonly IPostCategoryRepository _postCategoryRepository;

        public GetAllPostsWithCategoriesHandler(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }

        public async Task<IEnumerable<PostWithCategoriesDto>> Handle(GetAllPostsWithCategoriesQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postCategoryRepository.GetAllPostsWithCategoriesAsync();
            if (posts == null) return null;
            var result = posts
                .Select(p => new PostWithCategoriesDto
                {
                    Post = p,
                    Categories = p.Categories
                }).ToList();

            // Flatten the list of categories from all posts
            return result;
        }
    }

}
