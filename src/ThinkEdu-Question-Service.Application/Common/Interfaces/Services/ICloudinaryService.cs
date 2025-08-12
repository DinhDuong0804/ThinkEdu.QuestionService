using Microsoft.AspNetCore.Http;

namespace ThinkEdu_Question_Service.Application.Common.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<string> ConvertFileToUrlAsync(IFormFile file);

        Task<bool> DeleteFileByUrlAsync(string fileUrl);
    }
}
