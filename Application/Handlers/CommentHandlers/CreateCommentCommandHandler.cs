﻿using Application.Interfaces;
using Application.Queries.CommentCommands;
using Core.Models;
using MediatR;

namespace Application.Handlers.CommentHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentModel>
    {
        private readonly ICommentRepository _commentRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<CommentModel> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new CommentModel
            {
                CommentContent = request.CommentContent,
                UserId = request.UserId,
                PostId = request.PostId,
                ParentCommentId = request.ParentCommentId
            };

            await _commentRepository.AddAsync(comment);
            return comment;
        }
    }
}
