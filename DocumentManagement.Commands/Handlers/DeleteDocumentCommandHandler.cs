using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Commands.Commands;
using DocumentManagement.Commands.Common.ExecutionHistory;
using DocumentManagement.Commands.Results;
using DocumentManagement.Domain;
using MediatR;

namespace DocumentManagement.Commands.Handlers
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, CommandResult>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBlobDataService _blobDataService;
        private readonly IExecutionHistoryStore _historyStore;

        public DeleteDocumentCommandHandler(IDocumentRepository documentRepository, 
            IBlobDataService blobDataService, IExecutionHistoryStore historyStore)
        {
            _documentRepository = documentRepository;
            _blobDataService = blobDataService;
            _historyStore = historyStore;
        }

        public async Task<CommandResult> Handle(DeleteDocumentCommand request, 
            CancellationToken cancellationToken)
        {
            var document = await _documentRepository.Get(request.FileName);

            if (!await _blobDataService.Exists(request.FileName) || document == null)
            {
                return CommandResult.GetFailed("File is not found.");
            }

            _documentRepository.Remove(document);
            await _documentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            _historyStore.Add(ExecSteps.Documents.Delete.DbRecordDeleted, document);

            await _blobDataService.Delete(request.FileName);
            _historyStore.Add(ExecSteps.Documents.Delete.BlobDeleted, request.FileName);

            return CommandResult.GetSuccess();
        }
    }
}
