using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;


namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDomain> GetUserByEmailAsync(
            string emailAddress)
        {
            var user =  await _dbContext.Users.FirstOrDefaultAsync(
                        x => x.Email == emailAddress);

            if(user is not null)
                return new UserDomain()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                };

            return new UserDomain();
        }

        public async Task<bool> IsDuplicateUserAsync(
            string emailAddress)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == emailAddress);
        }

        public async Task<bool> RegisterUserAsync(
            UserDomain userDomain)
        {
            var user
                 = userDomain.ToUserEntity();

            user.CreatedBy = "system";
            
            _dbContext.Users.Add(user);

            int rowsInserted = await _dbContext.SaveChangesAsync();
            
            return rowsInserted > 0;
        }
    }
}
