namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class User : AuditableEntity
    {

        public string FirstName {  get; set; }

        public string LastName {  get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive {  get; set; }

    }
}
