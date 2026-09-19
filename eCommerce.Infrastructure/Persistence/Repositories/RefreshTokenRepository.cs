using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;


namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _dbContext;

        public RefreshTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> AddAsync(
            RefreshTokenDomain refreshTokenDomain)
        {
            var refreshToken
                  = refreshTokenDomain.ToRefreshToken();

            _dbContext.RefreshTokens.Add(refreshToken);

           return _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshTokenDomain> GetRefreshTokenAsync(
            string refreshToken)
        {
            var tokenDetail
                 = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenDetail == null)
                return null;

            return tokenDetail.ToRefreshTokenDomain();
        }
    }
}
