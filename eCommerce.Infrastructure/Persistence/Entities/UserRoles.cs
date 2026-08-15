namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class UserRoles : AuditableEntity
    {

        public int UserId { get; set; }

        public int RoleId { get; set; } 

   
    }
}
