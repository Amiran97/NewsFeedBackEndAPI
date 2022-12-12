using BackEnd.API.Models;
using BackEnd.API.Models.Dtos;

namespace BackEnd.API.Utils.Mappers
{
    public static class CommentMapper
    {
        public static ICollection<CommentResponse> ToCommentResponseCollection(ICollection<Comment> comments, int postId)
        {
            var result = new List<CommentResponse>();
            comments.ToList().ForEach(item =>
            {
                var likesResult = LikeMapper.ToStringCollection(item.Likes);
                result.Add(new CommentResponse
                {
                    Id = item.Id,
                    AuthorName = item.Author.UserName,
                    CreateAt = item.CreateAt,
                    Message = item.Message,
                    PostId = postId,
                    Likes = likesResult
                });
            });
            return result;
        }
    }
}
