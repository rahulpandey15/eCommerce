using eCommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configuration
{
    public class RoleConfiguration :
        IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
              .IsRequired()
              .HasDefaultValue(true);
        }
    }
}
