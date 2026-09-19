using eCommerce.Domain.DomainObjects;

namespace eCommerce.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<bool> RegisterUserAsync(UserDomain userDomain);
        Task<bool> IsDuplicateUserAsync(string emailAddress);
        Task<UserDomain> GetUserByEmailAsync(string emailAddress);

        Task<UserDomain> GetUserByIdAsync(int userId);
    }
}
