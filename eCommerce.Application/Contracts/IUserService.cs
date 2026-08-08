using eCommerce.Application.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Contracts
{
    public interface IUserService
    {
        // register user
        Task<bool> RegisterUserAsync(CreateUserDto createUser);
    }
}
