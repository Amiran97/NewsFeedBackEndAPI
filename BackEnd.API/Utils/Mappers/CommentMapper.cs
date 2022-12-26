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
                result.Add(ToCommentResponse(item, postId));
            });
            return result;
        }

        public static CommentResponse ToCommentResponse(Comment comment, int postId)
        {
            var likesResult = LikeMapper.ToStringCollection(comment.Likes);
            var dislikesResult = LikeMapper.ToStringCollection(comment.Dislikes);
            var result = new CommentResponse() {
                Id = comment.Id,
                AuthorName = comment.Author.UserName,
                CreateAt = comment.CreateAt,
                Message = comment.Message,
                PostId = postId,
                Likes = likesResult,
                Dislikes = dislikesResult,
                Rating = likesResult.Count - dislikesResult.Count
            };
            return result;
        }
    }
}
