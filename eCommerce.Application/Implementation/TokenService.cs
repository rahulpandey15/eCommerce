using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using eCommerce.Application.Exceptions;
using eCommerce.Application.Utils;
using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


namespace eCommerce.Application.Implementation
{
    public class TokenService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository)
        : ITokenService
    {
        public async Task<TokenResponseDto> GetTokenAsync(
            ValidateUserDto validateUserDto)
        {

            // validation
            // logic to fetch from database and compare

            var userDomainObj
                 = await userRepository.GetUserByEmailAsync(validateUserDto.userName);

            if (userDomainObj == null)
                throw new InvalidUserException("Invalid Credentials");


            bool isPasswordValid = passwordHasher.Verify(validateUserDto.password, userDomainObj.Password);

            if (!isPasswordValid)
                throw new InvalidPasswordException("Invalid Credentials");

            string refreshToken
                 = await GenerateAndStoreRefreshToken(userDomainObj);

            return new TokenResponseDto(GenerateAccessToken(userDomainObj), refreshToken);// give to refresh token
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(
            RefreshTokenDto refreshTokenDto)
        {
            if (string.IsNullOrEmpty(refreshTokenDto.accessToken))
                throw new InvalidTokenException("Access token is required.");

            if (string.IsNullOrEmpty(refreshTokenDto.refreshToken))
                throw new InvalidTokenException("Refresh token is required.");

            var storedToken
                  = await refreshTokenRepository.GetRefreshTokenAsync(refreshTokenDto.refreshToken);

            if(storedToken == null)
                throw new InvalidTokenException("Invalid Refresh Token");

            if (storedToken.IsExpired())
                throw new InvalidTokenException("Refresh Token Expired");

            if(storedToken.IsRevoked())
                throw new InvalidTokenException("Refresh Token Revoked"); // TODO : write a logic to revoke all token

            var userDomain = await userRepository.GetUserByIdAsync(storedToken.UserId);

            string refreshToken
               = await GenerateAndStoreRefreshToken(userDomain);

            return new TokenResponseDto(GenerateAccessToken(userDomain), refreshToken);
        }

        private string GenerateAccessToken(
            UserDomain userDomain)
        {
            var secretKey = configuration["Jwt:Secret"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var signingCredentials
                 = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var tokenDescriptor
                 = new SecurityTokenDescriptor
                 {
                     Subject = new System.Security.Claims.ClaimsIdentity([
                            new Claim(ClaimTypes.Name,userDomain.FirstName + " " + userDomain.LastName),
                            new Claim(ClaimTypes.Email,userDomain.Email)
                         ]),
                     Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["Jwt:TokenExpiryInMinutes"])),
                     SigningCredentials = signingCredentials,
                     Issuer = configuration["Jwt:Issuer"],
                     Audience = configuration["Jwt:Audience"]
                 };


            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }


        private async Task<string> GenerateAndStoreRefreshToken(UserDomain userDomain)
        {
            string rawToken
                 = RefreshTokenUtility.GenerateRefreshToken();

            // write a logic to store in database

            var refreshTokenDomain
                 = new RefreshTokenDomain
                 {
                     UserId = userDomain.UserId,
                     Token = rawToken,
                     ExpireAt = DateTime.UtcNow.AddDays(7),
                     RevokeAt = null
                 };

            await refreshTokenRepository.AddAsync(refreshTokenDomain);

            return rawToken;
        }

    }
}
