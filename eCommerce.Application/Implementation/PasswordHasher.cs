using eCommerce.Application.Contracts;

namespace eCommerce.Application.Implementation
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {   
            var hashedPassword = 
                BCrypt.Net.BCrypt.HashPassword(password); // salt is a kind of secret key
            return hashedPassword;
        }

        public bool Verify(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
