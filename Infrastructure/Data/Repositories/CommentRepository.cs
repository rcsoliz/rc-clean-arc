using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Comment entity, CancellationToken cancellationToken)
        {
            await _context.Comments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Comments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentByPostId(int Id, CancellationToken cancellationToken)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == Id)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto { 
                    Username = c.User.Username, 
                    CommentContent = c.CommentContent, 
                    UserId = c.UserId, 
                    PostId = c.PostId, 
                    Id = c.Id })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(new object[] {id}, cancellationToken);
            if (comment == null) return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
