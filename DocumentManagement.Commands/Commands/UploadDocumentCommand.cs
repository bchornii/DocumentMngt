using DocumentManagement.Commands.Results;
using MediatR;

namespace DocumentManagement.Commands.Commands
{
    public class UploadDocumentCommand : IRequest<CommandResult<string>>
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public byte[] Content { get; set; }
    }
}
