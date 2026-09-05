namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class Role : AuditableEntity
    {
        public string RoleName {  get; set; }

        public bool IsActive {  get; set; }

    }
}
