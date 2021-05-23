using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Commands.Commands;
using DocumentManagement.Commands.Common.ExecutionHistory;
using DocumentManagement.Commands.Results;
using DocumentManagement.Domain;
using MediatR;

namespace DocumentManagement.Commands.Handlers
{
    public class UploadDocumentCommandHandler : IRequestHandler<
        UploadDocumentCommand, CommandResult<string>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBlobDataService _blobDataService;
        private readonly IExecutionHistoryStore _historyStore;

        public UploadDocumentCommandHandler(IDocumentRepository documentRepository, 
            IBlobDataService blobDataService, IExecutionHistoryStore historyStore)
        {
            _documentRepository = documentRepository;
            _blobDataService = blobDataService;
            _historyStore = historyStore;
        }

        public async Task<CommandResult<string>> Handle(UploadDocumentCommand request, 
            CancellationToken cancellationToken)
        {
            var location = await _blobDataService.Upload(request.FileName, request.Content);
            _historyStore.Add(ExecSteps.Documents.Upload.BlobUploaded, request.FileName);       // TODO: it could be implemented based on AOP
                                                                                                // TODO: for ex. by using Interceptors

            var document = _documentRepository.Add(new Document
            {
                Name = request.FileName,
                Location = location,
                Size = request.Size,
                CreatedAt = DateTime.UtcNow
            });
            await _documentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            _historyStore.Add(ExecSteps.Documents.Upload.DbRecordAdded, document.Id);

            return CommandResult<string>.GetSuccess(location);
        }
    }
}
