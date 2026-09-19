using System.Security.Cryptography;

namespace eCommerce.Application.Utils
{
    public static class RefreshTokenUtility
    {
        public static string GenerateRefreshToken()
        {
            using(var random
                 = RandomNumberGenerator.Create())
            {
                var tokenBytes
                     = new byte[32];

                random.GetBytes(tokenBytes);

                return Convert.ToBase64String(tokenBytes)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .Replace("=", "");
            }
            
        }

    }
}
