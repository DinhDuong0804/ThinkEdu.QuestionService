using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Moq;
using ThinkEdu_Question_Service.Infrastructure.Services;
using GcsObject = Google.Apis.Storage.v1.Data.Object;

namespace UnitTest;

public class FirebaseUploadServiceTests
{
    private readonly Mock<StorageClient> _mockStorageClient;
    private readonly FirebaseUploadService _service;
    private const string ProjectId = "thinkeduquestionservice";
    private const string BucketName = ProjectId + ".appspot.com";

    public FirebaseUploadServiceTests()
    {
        _mockStorageClient = new Mock<StorageClient>();
        _service = new FirebaseUploadService(_mockStorageClient.Object);
    }

    [Fact]
    public async Task ConvertFileToUrlAsync_ReturnsNull_WhenFileIsNull()
    {
        var result = await _service.ConvertFileToUrlAsync(null);
        Assert.Null(result);
    }

    [Fact]
    public async Task ConvertFileToUrlAsync_ReturnsNull_WhenFileIsEmpty()
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        var result = await _service.ConvertFileToUrlAsync(mockFile.Object);

        Assert.Null(result);
    }

    [Fact]
    public async Task ConvertFileToUrlAsync_ReturnsUrl_WhenUploadSucceeds()
    {
        var fileName = "test.png";
        var contentType = "image/png";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };
        var stream = new MemoryStream(fileContent);

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(fileContent.Length);
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.ContentType).Returns(contentType);
        mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

        _mockStorageClient
           .Setup(c => c.UploadObjectAsync(
               It.IsAny<string>(),         // bucket  
               It.IsAny<string>(),         // objectName  
               It.IsAny<string>(),         // contentType  
               It.IsAny<Stream>(),         // source  
               null,                       // UploadObjectOptions  
               It.IsAny<CancellationToken>(), // CancellationToken  
               null))                      // IProgress<IUploadProgress>  
           .ReturnsAsync(new GcsObject());

        var result = await _service.ConvertFileToUrlAsync(mockFile.Object, "uploads");

        Assert.NotNull(result);
        Assert.Contains(BucketName, result);
        Assert.Contains("https://firebasestorage.googleapis.com/v0/b/", result);
        Assert.Contains("?alt=media", result);
    }

}