using BackEnd.API.Models;

namespace BackEnd.API.Utils.Mappers
{
    public static class LikeMapper
    {
        public static ICollection<string> ToStringCollection(ICollection<User> likes)
        {
            var result = new List<string>();
            likes.ToList().ForEach(pl =>
            {
                result.Add(pl.UserName);
            });
            return result;
        }
    }
}