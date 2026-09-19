
namespace eCommerce.Domain.DomainObjects
{
    public class RefreshTokenDomain
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public DateTime? RevokeAt { get; set; }


        public bool IsExpired() => DateTime.UtcNow > ExpireAt; // true if current datetime is more than token expiry date time

        public bool IsRevoked() => RevokeAt.HasValue && RevokeAt.Value <= DateTime.UtcNow; //

        public bool IsActive() => !IsExpired() && !IsRevoked();


    }
}
