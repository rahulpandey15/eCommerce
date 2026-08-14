using eCommerce.Domain.DomainObjects;

namespace eCommerce.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<bool> RegisterUserAsync(UserDomain userDomain);
        Task<bool> IsDuplicateUserAsync(string emailAddress);
        Task<UserDomain> GetUserByEmailAsync(string emailAddress);

        // Write a logic to register to user
        // write a logic to validate user 
    }
}
