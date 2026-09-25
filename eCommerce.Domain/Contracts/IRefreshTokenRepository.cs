using eCommerce.Domain.DomainObjects;

namespace eCommerce.Domain.Contracts
{
    public interface IRefreshTokenRepository
    {
        Task<int> AddAsync(RefreshTokenDomain refreshTokenDomain);

        Task<RefreshTokenDomain> GetRefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
