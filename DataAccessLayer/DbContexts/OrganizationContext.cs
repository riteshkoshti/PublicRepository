using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.DbContexts
{
    public partial class OrganizationContext : DbContext
    {
        private readonly DbContextOptions _options;

        public OrganizationContext(DbContextOptions<OrganizationContext> options)
            : base(options)
        {
            _options = options;
        }

        public virtual DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
