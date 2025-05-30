﻿using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.UpdatePostCategories
{
    public class UpdatePostCategoryCommandHandler : IRequestHandler<UpdatePostCategoryCommand, (Post post, List<int> categories)>
    {
        private readonly IPostCategoryRepository _postRepository;

        public  UpdatePostCategoryCommandHandler(IPostCategoryRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<(Post post, List<int> categories)> Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Id = request.Id,
                PostContent = request.PostContent,
                UserId = request.UserId,
                ImageUrl = request.ImageUrl,
            };
            await _postRepository.UpdateAsync(post, request.CategoryIds);

            return (post, request.CategoryIds.ToList());
        }

    }
}
