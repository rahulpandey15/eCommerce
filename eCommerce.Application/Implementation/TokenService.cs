using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using eCommerce.Application.Exceptions;
using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace eCommerce.Application.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IConfiguration configuration;

        public TokenService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.configuration = configuration;
        }


        public async Task<TokenResponseDto> GetTokenAsync(
            ValidateUserDto validateUserDto)
        {

            // validation
            // logic to fetch from database and compare

            var userDomainObj
                 = await userRepository.GetUserByEmailAsync(validateUserDto.userName);

            if (userDomainObj == null && userDomainObj.Email == null)
                throw new InvalidUserException("Invalid User");


            bool isPasswordValid = passwordHasher.Verify(validateUserDto.password, userDomainObj.Password);

            if (!isPasswordValid)
                throw new InvalidPasswordException("Invalid Password");


            return new TokenResponseDto(GenerateAccessToken(userDomainObj), "");
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
    }
}
