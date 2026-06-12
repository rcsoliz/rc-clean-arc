using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken=default);
        Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Comment entity,CancellationToken cancellationToken = default);
        Task<IEnumerable<CommentDto>> GetAllCommentByPostId(int Id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
