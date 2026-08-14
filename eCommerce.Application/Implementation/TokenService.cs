using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using eCommerce.Application.Exceptions;
using eCommerce.Domain.Contracts;


namespace eCommerce.Application.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;

        public TokenService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
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

            var hashedPassword = passwordHasher.Hash(validateUserDto.password);

            if (hashedPassword != userDomainObj.Password)
                throw new InvalidPasswordException("Invalid Password");


            return new TokenResponseDto("", "");
        }
    }
}
