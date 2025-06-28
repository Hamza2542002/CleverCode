using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using medical_app_db.Core.Helpers;
using medical_app_db.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace medical_app_db.EF.Services
{
    public class CloudinaryService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinatuSettings> cloudinaryOptions)
        {
            var account = new Account(
                cloudinaryOptions.Value.CloudName,
                cloudinaryOptions.Value.ApiKey,
                cloudinaryOptions.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string?> UploadImageAsync(IFormFile image)
        {
            using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = new Transformation()
                    .Width(800).Height(800).Crop("limit")  // Resize to limit
                    .Quality("auto")                       // Automatic compression
                    .FetchFormat("auto")                   // Automatic format (e.g., WebP)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
        public async Task<bool> DeleteImageAsync(Guid id)
        {
            var delteResult = await _cloudinary.DestroyAsync(new DeletionParams(id.ToString()));

            if (delteResult.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }
        public async Task<string?> UpdateImageAsync(IFormFile? image, Guid id)
        {
            if (id == Guid.Empty || image == null)
                return null;

            await DeleteImageAsync(id);

            return await UploadImageAsync(image);
        }
    }
}
