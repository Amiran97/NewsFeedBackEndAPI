using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.API.Models.Dtos
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AuthorName { get; set; }
        public ICollection<string> Likes { get; set; }
        public ICollection<string> Tags { get; set; }
        public int CommentCount { get; set; }
        public ICollection<string> Images { get; set; }
        
    }
}
