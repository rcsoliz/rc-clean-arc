using Core.Dtos;
using Core.Entities;
using Core.Models;

namespace Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task AddAsync(CommentModel entity);
        Task<IEnumerable<CommentDto>> GetAllCommentByPostId(int Id);
        Task DeleteAsync(int id);
    }
}
