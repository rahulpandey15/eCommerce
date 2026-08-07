using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Entities
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }


       public DbSet<User> Users { get; set; }

       public DbSet<Role> Roles { get; set; }

       public DbSet<UserRoles> UserRoles { get; set; }
    }
}
