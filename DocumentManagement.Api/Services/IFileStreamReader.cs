using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.Api.Services
{
    public interface IFileStreamReader
    {
        Task<FileStreamReadResult> Read(Stream stream, string contentType);
    }
}