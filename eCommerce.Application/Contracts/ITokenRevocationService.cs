
namespace eCommerce.Application.Contracts
{
    public interface ITokenRevocationService
    {
        Task<bool> IsSessionRevokedAsync(string sessionId);  // this will check if session is revoked or not
        Task InvalidateSessionCacheAsync(string sessionId);  // this is invalidate the cache
    }
}
