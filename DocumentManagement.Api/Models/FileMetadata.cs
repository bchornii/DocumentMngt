using System.IO;

namespace DocumentManagement.Api.Models
{
    public class FileMetadata
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public Stream Content { get; set; }
    }
}
