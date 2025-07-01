
using HotelsBooking.BLL.Interfaces;

namespace HotelsBooking.BLL.Services
{
    public class ImageService
    {
        private readonly IImagePath _rootPath;
        public ImageService(IImagePath rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<string> UploadImageAsync(IImageFile image, string directory, CancellationToken ct = default)
        {
            if (!IsImage(image))
            {
                throw new ArgumentException("Переданный файл не является поддерживаемым изображением");
            }
            var uploadRoot = Path.Combine(_rootPath.RootPath, directory);
            Directory.CreateDirectory(uploadRoot);

            var uniqueFileName = $"{Guid.NewGuid()}-{image.FileName}";
            var filePath = Path.Combine(uploadRoot, uniqueFileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fs, ct);
            }

            return Path.Combine(directory, uniqueFileName);
        }

        public Task DeleteImageAsync(string imagePath, CancellationToken ct)
        {
            var filePath = Path.Combine (_rootPath.RootPath, imagePath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
        private static bool IsImage(IImageFile file)
        {
            var validImageTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
            return validImageTypes.Contains(file.ContentType);
        }
    }
}
