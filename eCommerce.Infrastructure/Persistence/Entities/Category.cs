using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
