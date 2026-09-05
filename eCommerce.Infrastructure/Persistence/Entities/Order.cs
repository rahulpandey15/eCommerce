using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Infrastructure.Persistence.Entities
{
    public class Order : AuditableEntity
    {
        public string OrderNumber {  get; set; }

        public decimal TotalAmount {  get; set; }

        public string Status {  get; set; }

        public int UserId {  get; set; }


        // Navigation Properties
        public User User { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }



    }
}
