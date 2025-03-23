using Application.Common;
using Core.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.PostCommands
{
    public class GetPagedPostsQuery : IRequest<PagedResult<PostDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
