using System.Threading.Tasks;
using DocumentManagement.Commands.Common;
using DocumentManagement.Domain;

namespace DocumentManagement.Commands.Handlers.Compensations.Commands
{
    [CompensationAction(Name = nameof(DeleteDocumentCommandHandler))]
    public class CompensateDeleteDocumentCommand : ICompensationAction
    {
        private readonly IExecutionHistoryStore _historyStore;
        private readonly IDocumentRepository _documentRepository;

        public CompensateDeleteDocumentCommand(IExecutionHistoryStore historyStore,
            IDocumentRepository documentRepository)
        {
            _historyStore = historyStore;
            _documentRepository = documentRepository;
        }
        public async Task Compensate()
        {
            // TODO: optionally this compensation could be based on blob soft delete
            var document = _historyStore.Get<Document>(
                ExecSteps.Documents.Delete.DbRecordDeleted);
            _documentRepository.Add(document);
            await _documentRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}