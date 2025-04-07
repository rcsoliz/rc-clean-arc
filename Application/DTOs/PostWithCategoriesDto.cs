using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PostWithCategoriesDto
    {
        public PostDto Post { get; set; }
        public IEnumerable<PostCategoryDtos> Categories { get; set; }
    }
}
