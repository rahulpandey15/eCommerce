namespace eCommerce.Infrastructure.Entities
{
    public class Role : AuditableEntity
    {
        public string RoleName {  get; set; }

        public bool IsActive {  get; set; }

    }
}
