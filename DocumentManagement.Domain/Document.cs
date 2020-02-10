using System;

namespace DocumentManagement.Domain
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Extenstion { get; set; }
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
    }
}
