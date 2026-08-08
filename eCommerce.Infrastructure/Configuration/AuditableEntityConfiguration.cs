using Microsoft.EntityFrameworkCore;
using eCommerce.Infrastructure.Entities;

namespace eCommerce.Infrastructure.Configuration
{
    // Applies common configuration for all entities deriving from AuditableEntity
    public static class AuditableEntityConfiguration
    {
        public static void ApplyAuditableEntityConfiguration(this ModelBuilder modelBuilder)
        {
            var auditableBase = typeof(AuditableEntity);

            var auditableEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType != null && auditableBase.IsAssignableFrom(t.ClrType))
                .Select(t => t.ClrType)
                .ToList();

            foreach (var clrType in auditableEntityTypes)
            {
                var entity = modelBuilder.Entity(clrType);

                // Ensure Id is key
                entity.HasKey("Id");

                // CreatedBy - required, max length 100
                entity.Property<string>("CreatedBy")
                      .IsRequired()
                      .HasMaxLength(100);

                // CreatedOn - required, default to current UTC time at DB side
                entity.Property<DateTime>("CreatedOn")
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                // ModifiedBy - optional, max length 100
                entity.Property<string?>("ModifiedBy")
                      .HasMaxLength(100);

                // ModifiedOn - optional
                entity.Property<DateTime?>("ModifiedOn");
            }
        }
    }
}
