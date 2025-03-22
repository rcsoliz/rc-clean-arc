using Core.Dtos;
using Core.Entities;


namespace Core.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task AddAsync(Post entity);

        Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync();

    }
}
