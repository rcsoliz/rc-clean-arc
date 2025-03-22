using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Serivces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync();
    }
}
