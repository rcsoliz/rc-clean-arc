using Application.Common;
using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task AddAsync(Post entity);

        Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync();

        Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize);

        Task<PagedResult<PostDto>> GetPagedPostsAsyncRefactory(int page, int pageSize);
        IQueryable<Post> GetQueryableWithUserAndComments();
    }
}
