using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Models;
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
        public async Task AddAsync(CommentModel entity)
        {
            var comment = new Comment
            {
                UserId = entity.UserId,
                PostId = entity.PostId,
                CommentContent = entity.CommentContent,
                ParentCommentId = entity.ParentCommentId,
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentByPostId(int Id)
        {
            return await _context.Comments
                .Where(c => c.PostId == Id)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    CommentContent = c.CommentContent,
                    Username = c.User.Username,
                    UserId = c.UserId,
                    Created = c.CreatedAt.ToString(),
                    PostId = c.PostId,
                    ParentCommentId = c.ParentCommentId
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
