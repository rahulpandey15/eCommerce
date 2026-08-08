using Microsoft.EntityFrameworkCore;
using eCommerce.Infrastructure.Persistence.Configuration;

namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply common auditable entity configuration to all entities that inherit AuditableEntity
            modelBuilder.ApplyAuditableEntityConfiguration();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


       public DbSet<User> Users { get; set; }

       public DbSet<Role> Roles { get; set; }

       public DbSet<UserRoles> UserRoles { get; set; }
    }
}
