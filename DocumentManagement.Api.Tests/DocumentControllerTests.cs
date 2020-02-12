using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Api.Controllers;
using DocumentManagement.Api.Services;
using DocumentManagement.Commands.Commands;
using DocumentManagement.Commands.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DocumentManagement.Api.Tests
{
    [Trait("Category", nameof(DocumentControllerTests))]
    public class DocumentControllerTests
    {
        private DocumentControllerFixture Fixture { get; }

        public DocumentControllerTests()
        {
            Fixture = new DocumentControllerFixture();
        }

        [Fact]
        public async Task Upload_RequestBody_IsNull_ShouldReturn_BadRequest()
        {
            // Arrange
            Mock.Get(Fixture.FileStreamReader)
                .Setup(s => s.Read(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync((FileStreamReadResult) null);

            // Act
            var result = await Fixture.DocumentController.Upload();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Upload_ReadBodyResult_IsNotSuccessful_IsNull_ShouldReturn_BadRequest()
        {
            // Arrange
            var msg = "error msg";

            Mock.Get(Fixture.FileStreamReader)
                .Setup(s => s.Read(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(new FileStreamReadResult(false, msg, It.IsAny<string>(), It.IsAny<byte[]>()));

            // Act
            var result = await Fixture.DocumentController.Upload();

            // Assert
            var methodResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(methodResult.Value.ToString(), msg);
        }

        [Fact]
        public async Task Upload_RequestBody_IsRead_ShouldSend_UploadDocumentCommand()
        {
            // Arrange
            Mock.Get(Fixture.FileStreamReader)
                .Setup(s => s.Read(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(new FileStreamReadResult(true, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()));

            // Act
            var result = await Fixture.DocumentController.Upload();

            // Assert
            Mock.Get(Fixture.Mediator)
                .Verify(v => v.Send(It.IsAny<UploadDocumentCommand>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Upload_RequestBody_IsRead_And_Processed_ShouldReturn_Ok()
        {
            // Arrange
            var fileName = "filename.pdf";
            var content = new byte[124];

            Mock.Get(Fixture.FileStreamReader)
                .Setup(s => s.Read(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(new FileStreamReadResult(true, It.IsAny<string>(), fileName, content));

            // Act
            var result = await Fixture.DocumentController.Upload();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldCall_DeleteDocumentCommand_WithCorrectParameter()
        {
            // Arrange
            var fileName = "file name.pdf";

            // Act
            await Fixture.DocumentController.Delete(fileName);

            // Assert
            Mock.Get(Fixture.Mediator)
                .Verify(v => v.Send(It.Is<DeleteDocumentCommand>(
                    it => it.FileName == fileName), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Delete_ProcessedSuccessfully_ShouldReturn_NoContent()
        {
            // Arrange
            Mock.Get(Fixture.Mediator)
                .Setup(s => s.Send(It.IsAny<DeleteDocumentCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResult.GetSuccess());

            // Act
            var result = await Fixture.DocumentController.Delete(It.IsAny<string>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ProcessedWithError_ShouldReturn_BadRequest()
        {
            // Arrange
            var msg = "error message";

            Mock.Get(Fixture.Mediator)
                .Setup(s => s.Send(It.IsAny<DeleteDocumentCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResult.GetFailed(msg));

            // Act
            var result = await Fixture.DocumentController.Delete(It.IsAny<string>());

            // Assert
            var methodResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(msg, methodResult.Value.ToString());
        }

        private class DocumentControllerFixture
        {
            public IFileStreamReader FileStreamReader { get; }
            public IMediator Mediator { get; }
            public DocumentController DocumentController { get; }

            public DocumentControllerFixture()
            {
                FileStreamReader = Mock.Of<IFileStreamReader>();
                Mediator = Mock.Of<IMediator>();
                DocumentController = new DocumentController(FileStreamReader, Mediator)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                    }
                };
            }
        }
    }
}
