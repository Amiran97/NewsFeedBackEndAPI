using BackEnd.API.Models;

namespace BackEnd.API.Utils.Mappers
{
    public static class ImageMapper
    {
        public static ICollection<string> ToStringCollection(ICollection<PostImage> postImages)
        {
            var result = new List<string>();
            postImages.ToList().ForEach(item =>
            {
                result.Add(item.Path);
            });
            return result;
        }
    }
}
