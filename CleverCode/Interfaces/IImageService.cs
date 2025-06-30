using Microsoft.AspNetCore.Http;
namespace medical_app_db.Core.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile? image, string name);
        Task<string?> UpdateImageAsync(IFormFile? image, string name);
        Task<bool> DeleteImageAsync(string name);
    }
}
