namespace HotelsBooking.BLL.Interfaces
{
    public interface IImageService
    {
        Task DeleteImageAsync(string imagePath);
        Task<string> UploadImageAsync(IImageFile image, string directory, CancellationToken ct = default);
    }
}