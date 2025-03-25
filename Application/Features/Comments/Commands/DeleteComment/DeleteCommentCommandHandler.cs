using Application.Interfaces;
using MediatR;

namespace Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var product = await _commentRepository.GetByIdAsync(request.Id);
            if (product == null) return false;

            await _commentRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}
