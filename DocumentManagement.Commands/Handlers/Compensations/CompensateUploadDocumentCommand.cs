﻿using System.Threading.Tasks;
using DocumentManagement.Commands.Common.Compensations;
using DocumentManagement.Commands.Common.ExecutionHistory;
using DocumentManagement.Domain;

namespace DocumentManagement.Commands.Handlers.Compensations
{
    [CompensationAction(Name = nameof(UploadDocumentCommandHandler))]
    public class CompensateUploadDocumentCommand : ICompensationAction
    {
        private readonly IBlobDataService _blobDataService;
        private readonly IExecutionHistoryStore _historyStore;

        public CompensateUploadDocumentCommand(
            IBlobDataService blobDataService, IExecutionHistoryStore historyStore)
        {
            _blobDataService = blobDataService;
            _historyStore = historyStore;
        }

        /// <inheritdoc/>
        public async Task Compensate()
        {
            var blobLocation = _historyStore.Get<string>(
                ExecSteps.Documents.Upload.BlobUploaded);

            if (!string.IsNullOrEmpty(blobLocation))
            {
                await _blobDataService.Delete(blobLocation);
            }
        }
    }
}