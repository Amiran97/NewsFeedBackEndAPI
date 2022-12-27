using BackEnd.API.Models;
using MediatR;
using static System.Net.Mime.MediaTypeNames;

namespace BackEnd.API.Utils
{
    public static class ImageHelper
    {
        static ImageHelper()
        {
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
        }

        private static readonly string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private static readonly string imagesFolder = Path.Combine(rootFolder, "Images");

        public static async Task<string> SaveImage(IFormFile file)
        {
            var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(imagesFolder, newName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                return newName;
            }
            throw new Exception("Can't save file");
        }

        public static async Task<ICollection<string>> SaveImages(IFormFileCollection files)
        {
            ICollection<string> result = new List<string>();
            foreach (var file in files)
            {
                var item = await SaveImage(file);
                result.Add(item);
            }
            return result;
        }

        public static void RemoveImage(string filePath)
        {
            var fullPath = Path.Combine(imagesFolder, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public static void RemoveImages(ICollection<string> filesPath)
        {
            filesPath.ToList().ForEach(filePath => RemoveImage(filePath));
        }
    }
}
