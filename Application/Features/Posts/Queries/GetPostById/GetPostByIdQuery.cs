﻿using Application.DTOs;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Queries.GetPostById
{
    public record GetPostByIdQuery(int id) : IRequest<PostDto>;

}
