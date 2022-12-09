using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.API.Models.Dtos
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public string AuthorName { get; set; }
        public ICollection<string> Likes { get; set; }
    }
}
