using FastEndpoints;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Application.Models;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Uploads.Commands
{
    public class UploadImageCommand : Endpoint<UploadFileRequest,string>
    {
        private readonly ICloudinaryService _cloudinaryService;

        public UploadImageCommand(
            ICloudinaryService cloudinaryService
            )
        {
            _cloudinaryService = cloudinaryService;
        }

        public override void Configure()
        {
            Post("/Uploads/Image");
            AllowAnonymous();
            AllowFormData();
            AllowFileUploads();
            Summary(summary =>
            {
                summary.Summary = "Upload hình ảnh hoăc video lên Cloudinary";
            });
        }

        public override async Task HandleAsync(UploadFileRequest req, CancellationToken ct)
        {
            if (ValidationFailed)
            {
                return;
            }
            var response = await _cloudinaryService.ConvertFileToUrlAsync(req.File);
            if (string.IsNullOrEmpty(response))
            {
                
                throw new BadRequestException("File upload failed. Please try again.");
            }
            await SendOkAsync(response, ct);
        }
    }
}
