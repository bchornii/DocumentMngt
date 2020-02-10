using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.Domain
{
    public interface IBlobDataService
    {
        Task<bool> Exists(string name);
        Task<string> Upload(string name, byte[] content);
        Task Delete(string name);
    }
}