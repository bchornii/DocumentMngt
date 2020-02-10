using System.Threading.Tasks;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentDbContext _dbContext;

        public IUnitOfWork UnitOfWork => _dbContext;

        public DocumentRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Document> Get(string name)
        {
            return _dbContext.Documents.FirstOrDefaultAsync(d => d.Name == name);
        }

        public Document Add(Document document)
        {
            return _dbContext.Documents.Add(document)?.Entity;
        }

        public void Remove(Document document)
        {
            _dbContext.Documents.Remove(document);
        }
    }
}
