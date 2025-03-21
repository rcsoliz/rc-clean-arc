using Core.Entities;


namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task<string> AddAsync(Comment entity);

    }
}
