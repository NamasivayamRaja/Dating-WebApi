using API.Helper;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudAccount;
        public PhotoService(IOptions<CloudinarySettings> options)
        {
            var acc = new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.APiSecret);
            _cloudAccount = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                    Folder="do-8"
                };

               uploadResult = await _cloudAccount.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            
            return await _cloudAccount.DestroyAsync(deleteParams);
        }
    }
}
