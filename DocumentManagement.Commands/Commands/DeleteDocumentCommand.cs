using DocumentManagement.Commands.Results;
using MediatR;

namespace DocumentManagement.Commands.Commands
{
    public class DeleteDocumentCommand : IRequest<CommandResult>
    {
        public string FileName { get; set; }
    }
}
