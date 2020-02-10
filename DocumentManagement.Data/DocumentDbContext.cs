using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Data
{
    public class DocumentDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Document> Documents { get; set; }

        public DocumentDbContext(DbContextOptions<
            DocumentDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
