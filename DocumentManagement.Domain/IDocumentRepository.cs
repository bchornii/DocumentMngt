using System.Threading.Tasks;

namespace DocumentManagement.Domain
{
    public interface IDocumentRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Document> Get(string name);
        Document Add(Document document);
        void Remove(Document document);
    }
}