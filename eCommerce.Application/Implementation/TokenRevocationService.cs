using eCommerce.Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Implementation
{
    internal class TokenRevocationService : ITokenRevocationService
    {
        private readonly IDistributedCache _distributedCache;

        public TokenRevocationService(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
        }


        public async Task InvalidateSessionCacheAsync(string sessionId)
        {
            var key = $"session:revoked:{sessionId}";

            await _distributedCache.SetStringAsync(key, "1");
        }

        public async Task<bool> IsSessionRevokedAsync(string sessionId)
        {
            var key = $"session:revoked:{sessionId}";

            var cached = await _distributedCache.GetStringAsync(key);

            if (cached != "[]")
                return cached == "1";  // currently my redis cached is empty 

            // we have to insert revoked id as cache


            return false;
        }
    }
}
