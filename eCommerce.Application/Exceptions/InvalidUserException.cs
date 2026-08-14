using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string message) : base(message)
        {
            
        }
    }
}
