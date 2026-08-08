using BCrypt.Net;
using eCommerce.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
