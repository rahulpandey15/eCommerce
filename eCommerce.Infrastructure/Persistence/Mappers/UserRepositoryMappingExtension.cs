using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;

namespace eCommerce.Infrastructure.Persistence.Mappers
{
    internal static class UserRepositoryMappingExtension
    {
        public static User ToUserEntity(
            this UserDomain userDomain)
        {
            return new User()
            {
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Password = userDomain.Password,
                Email = userDomain.Email,
            };
        }
    }
}
