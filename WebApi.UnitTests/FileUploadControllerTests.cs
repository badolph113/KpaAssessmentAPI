using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using WebApi.Application.Services;
using WebApi.Domain.Entities;
using WebApi.Web.Controllers;
using WebApi.Web.Validators;

namespace WebApi.Tests
{
    public class JsonFilesControllerTests
    {
        private readonly Mock<IJsonFilesService> _mockJsonDataService;
        private readonly JsonFilesController _controller;

        public JsonFilesControllerTests()
        {
            _mockJsonDataService = new Mock<IJsonFilesService>();
            _controller = new JsonFilesController(_mockJsonDataService.Object);
        }

        [Fact]
        public async Task GetJsonFilesIdsAsync_ReturnsOkResult_WithListOfIds()
        {
            // Arrange
            var mockIds = new List<string> { "id1", "id2" };
            _mockJsonDataService.Setup(service => service.GetAllJsonFilesIdsAsync())
                .ReturnsAsync(mockIds);

            // Act
            var result = await _controller.GetJsonFilesIdsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedIds = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(mockIds.Count, returnedIds.Count());
        }

        [Fact]
        public async Task GetJsonFileByIdAsync_ReturnsOkResult_WithFile()
        {
            // Arrange
            string fileId = "testId";
            var mockFile = new JsonFile();
            _mockJsonDataService.Setup(service => service.GetJsonFileByIdAsync(fileId))
                .ReturnsAsync(mockFile);

            // Act
            var result = await _controller.GetJsonFileByIdAsync(fileId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockFile, okResult.Value);
        }

        [Fact]
        public async Task UploadJsonFileAsync_ReturnsBadRequest_WhenFileIsInvalid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var validator = new FileValidator();
            var validationResult = validator.Validate(mockFile.Object);

            // Act
            var result = await _controller.UploadJsonFileAsync(mockFile.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadJsonFileAsync_ReturnsOkResult_WhenFileIsValid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "{\"key\": \"value\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            mockFile.Setup(_ => _.OpenReadStream()).Returns(stream);
            mockFile.Setup(_ => _.Length).Returns(stream.Length);

            var jsonFile = new JsonFile { /* ... properties ... */ };
            _mockJsonDataService.Setup(service => service.ProcessJsonDataAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonFile);

            // Act
            var result = await _controller.UploadJsonFileAsync(mockFile.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(jsonFile, okResult.Value);
        }

        [Fact]
        public async Task UpdateJsonFileAsync_ReturnsOkResult_WhenFileIsUpdated()
        {
            // Arrange
            string fileId = "validId";
            var content = "{\"newKey\": \"newValue\"}";
            var updatedJsonFile = new JsonFile { /* ... properties ... */ };
            _mockJsonDataService.Setup(service => service.UpdateJsonFileAsync(fileId, content))
                .ReturnsAsync(updatedJsonFile);

            // Act
            var result = await _controller.UpdateJsonFileAsync(fileId, content);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedJsonFile, okResult.Value);
        }
    }
}