using BackEnd.API.Models;

namespace BackEnd.API.Utils.Mappers
{
    public static class TagMapper
    {
        public static ICollection<string> ToStringCollection(ICollection<Tag> tags)
        {
            var result = new List<string>();
            foreach (var tag in tags)
            {
                result.Add(tag.Name);
            }
            return result;
        }
    }
}