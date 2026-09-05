using eCommerce.Application.DTO.Request;
using eCommerce.Domain.DomainObjects;

namespace eCommerce.Application.Mappers
{
    public static class UserServiceMappingExtension
    {
        public static UserDomain ToUserDomain(
            this CreateUserDto userDto)
        {
            return new UserDomain
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = userDto.Password,
            };
        }

    }
}
