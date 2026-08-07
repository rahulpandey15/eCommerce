using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Infrastructure.Entities
{
    public class UserRoles : AuditableEntity
    {

        public int UserId { get; set; }

        public int RoleId { get; set; } 
    }
}
