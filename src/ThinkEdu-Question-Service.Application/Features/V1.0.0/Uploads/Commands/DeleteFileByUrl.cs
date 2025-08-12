using FastEndpoints;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Uploads.Commands
{
    public class DeleteFileRequest
    {
        public string? FileUrl { get; set; }
    }

    public class DeleteFileByUrl : Endpoint<DeleteFileRequest, BaseResponse<bool>>
    {
        private readonly ICloudinaryService _cloudinaryService;

        public DeleteFileByUrl(
            ICloudinaryService cloudinaryService
            )
        {
            _cloudinaryService = cloudinaryService;
        }

        public override void Configure()
        {
            Delete("/Uploads/Image");
            AllowAnonymous();
            //AllowFormData();
            AllowFileUploads();
            Summary(summary =>
            {
                summary.Summary = "xóa hình ảnh hoăc video lên Cloudinary";
            });
        }

        public override async Task HandleAsync(DeleteFileRequest req, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(req.FileUrl))
            {
                throw new BadRequestException("File URL is required.");
            }
            var response = await _cloudinaryService.DeleteFileByUrlAsync(req.FileUrl);
            if (!response)
            {
                throw new BadRequestException("File delete failed. Please try again.");
            }

            var baseResponse = new BaseResponse<bool>
            {
                Data = response,
                Success = true,
                Message = "File deleted successfully."
            };

            await SendOkAsync(baseResponse, ct);
        }
    }
}