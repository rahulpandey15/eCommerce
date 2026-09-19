
namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class RefreshToken : AuditableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public DateTime? RevokeAt { get; set; }


    }
}
