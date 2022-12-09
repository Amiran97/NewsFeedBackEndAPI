using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.API.Models.Dtos
{
    public class PostsResponse
    {
        public IEnumerable<PostResponse> Posts { get; set;}
        public int TotalCount { get; set;}
        public int TotalPages { get; set;}
        public int Page { get; set;}
    }
}
