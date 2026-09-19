using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;

namespace eCommerce.Infrastructure.Persistence.Mappers
{
    public static class RefreshTokenRepositoryMappingExtension
    {
        public static RefreshToken ToRefreshToken(
            this RefreshTokenDomain domain)
        {
            return new RefreshToken()
            {
                UserId = domain.UserId,
                Token = domain.Token,
                ExpireAt = domain.ExpireAt,
                RevokeAt = domain.RevokeAt,
                CreatedBy = "system",
                CreatedOn = System.DateTime.UtcNow
            };
        }

        public static RefreshTokenDomain ToRefreshTokenDomain(
            this RefreshToken domain)
        {
            return new RefreshTokenDomain()
            {
                UserId = domain.UserId,
                Token = domain.Token,
                ExpireAt = domain.ExpireAt,
                RevokeAt = domain.RevokeAt,
            };
        }

    }
}
