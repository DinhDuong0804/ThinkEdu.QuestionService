using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Domain.Configurations;

namespace ThinkEdu_Question_Service.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<string> ConvertFileToUrlAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null!;

            using (var stream = file.OpenReadStream())
            {
                var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

                // Kiểm tra nếu là video  hoặc âm thanh
                if (fileExtension == ".mp4" || fileExtension == ".mp3" || fileExtension == ".mov" || fileExtension == ".avi" || fileExtension == ".webm")
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl?.ToString()!;
                }
                else
                {
                    // Mặc định là ảnh
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl?.ToString()!;
                }
            }
        }

        public async Task<bool> DeleteFileByUrlAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return false;

            try
            {
                var uri = new Uri(fileUrl);
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                // Tìm vị trí "upload"
                int uploadIndex = Array.FindIndex(segments, s => s.Equals("upload", StringComparison.OrdinalIgnoreCase));
                if (uploadIndex < 0 || uploadIndex + 1 >= segments.Length)
                    return false;

                // Bỏ qua version nếu có (bắt đầu bằng 'v' và là số)
                var publicIdSegments = segments.Skip(uploadIndex + 1).ToList();
                if (publicIdSegments.Count > 1 && publicIdSegments[0].StartsWith("v") && long.TryParse(publicIdSegments[0].Substring(1), out _))
                {
                    publicIdSegments = publicIdSegments.Skip(1).ToList();
                }

                // Nối lại và bỏ extension
                var publicIdWithExt = string.Join('/', publicIdSegments);
                var lastDot = publicIdWithExt.LastIndexOf('.');
                var publicId = lastDot > 0 ? publicIdWithExt.Substring(0, lastDot) : publicIdWithExt;

                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);
                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }
    }
}