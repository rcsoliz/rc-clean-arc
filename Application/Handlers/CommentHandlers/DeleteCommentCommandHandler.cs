﻿using Application.Interfaces;
using Application.Queries.CommentCommands;
using MediatR;

namespace Application.Handlers.CommentHandlers
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
