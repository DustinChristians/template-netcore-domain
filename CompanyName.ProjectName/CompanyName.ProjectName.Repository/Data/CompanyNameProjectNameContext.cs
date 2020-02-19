using CompanyName.ProjectName.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Data
{
    public class CompanyNameProjectNameContext : DbContext
    {
        public CompanyNameProjectNameContext(DbContextOptions<CompanyNameProjectNameContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        // When the database is created, EF creates tables that have names the same as the DbSet property names.
        // Property names for collections are typically plural (Students rather than Student)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable(nameof(Message));
        }
    }
}
