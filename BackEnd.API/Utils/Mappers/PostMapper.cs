using BackEnd.API.Models;
using BackEnd.API.Models.Dtos;

namespace BackEnd.API.Utils.Mappers
{
    public static class PostMapper
    {
        public static PostResponse ToPostResponse(Post post)
        {
            var likesResult = LikeMapper.ToStringCollection(post.Likes);
            var tags = TagMapper.ToStringCollection(post.Tags);
            var images = ImageMapper.ToStringCollection(post.Images);
            var result = new PostResponse()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                AuthorName = post.Author.UserName,
                Likes = likesResult,
                CommentCount = post.Comments.Count,
                Tags = tags,
                Images = images
            };
            return result;
        }
        public static PostsResponse ToPostsResponse(ICollection<Post> posts, int totalCount, int totalPages, int page) 
        {
            var tempPosts = new List<PostResponse>();
            posts.ToList().ForEach(item =>
            {
                var post = ToPostResponse(item);
                tempPosts.Add(post);
            });
            var result = new PostsResponse()
            {
                Posts = tempPosts,
                Page = page,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return result;
        }
    }
}