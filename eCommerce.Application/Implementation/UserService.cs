using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.Mappers;
using eCommerce.Domain.Contracts;

namespace eCommerce.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterUserAsync(CreateUserDto createUser)
        {
            // validation

            var userDomain = createUser.ToUserDomain();
            userDomain.Password = _passwordHasher.Hash(userDomain.Password);


            bool isDuplicateUser
                 = await _userRepository.IsDuplicateUserAsync(userDomain.Email);

            if(isDuplicateUser)
            {
                // throw an exception
            }

            var response
                 = await _userRepository.RegisterUserAsync(userDomain);


            return response;
        }
    }
}
