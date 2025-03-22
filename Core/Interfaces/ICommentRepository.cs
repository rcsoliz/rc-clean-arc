using Core.Entities;
using Core.Models;


namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task AddAsync(CommentModel entity);

    }
}
