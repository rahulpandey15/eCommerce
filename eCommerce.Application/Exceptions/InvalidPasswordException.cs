using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {

        }
    }
}
