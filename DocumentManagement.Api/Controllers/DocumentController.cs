using System.Threading.Tasks;
using DocumentManagement.Api.Infrastructure.Files;
using DocumentManagement.Api.Services;
using DocumentManagement.Commands.Commands;
using DocumentManagement.Commands.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IFileStreamReader _fileStreamReader;
        private readonly IMediator _mediator;

        public DocumentController(IFileStreamReader fileStreamReader, 
            IMediator mediator)
        {
            _fileStreamReader = fileStreamReader;
            _mediator = mediator;
        }

        [HttpPost("upload")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Upload()
        {
            var readResult = await _fileStreamReader
                .Read(Request.Body, Request.ContentType);

            if (readResult == null)
            {
                return BadRequest("Please select file to upload.");
            }

            if (!readResult.Success)
            {
                return BadRequest(readResult.Error);
            }

            var result = await _mediator.Send(
                new UploadDocumentCommand
                {
                    FileName = readResult.FileName,
                    Size = readResult.Size,
                    Content = readResult.Content
                });

            return Ok(new
            {
                readResult.FileName,
                readResult.Size,
                Location = result
            });
        }

        [HttpPost("delete/{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            var result = await _mediator
                .Send(new DeleteDocumentCommand
                {
                    FileName = fileName
                });

            return result.Status == CommandResultStatus.Fail 
                ? BadRequest(result.Message) 
                : (IActionResult) NoContent();
        }
    }
}