using Microsoft.AspNetCore.Http;

namespace ThinkEdu_Question_Service.Application.Models
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
