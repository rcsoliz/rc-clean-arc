﻿using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IPostCategoryRepository
    {
        Task AddAsync(Post entity, List<int> categoryIds);

        Task UpdateAsync(Post entity, List<int> categoryIds);

        Task DeleteAsync(int id, List<int> categoryIds);

        Task<IEnumerable<PostDto>> GetAllPostsWithCategoriesAsync();

        Task<PostDto> GetPostByIdtWithCategoriesAsync(int id);

        Task<IEnumerable<PostDto>> GetPostWithCategoryIdAsync(int categoryId);

        Task<IEnumerable<PostWithCategoriesDto>> GetPostsByScrollAsync(DateTime? lastPostDate, int take = 5);

        Task<int> CountNewPostsAsync(DateTime afterDate);
        Task<IEnumerable<PostWithCategoriesDto>> GetNewPostsAfterAsync(DateTime afterDate);

    }
}
