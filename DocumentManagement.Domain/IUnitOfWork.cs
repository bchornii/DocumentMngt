using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Domain
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}