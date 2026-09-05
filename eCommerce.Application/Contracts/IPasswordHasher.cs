using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Contracts
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string plainTextPassword, string hashedPassword);

    }
}
