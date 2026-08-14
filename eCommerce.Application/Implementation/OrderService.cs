using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Implementation
{
    public class OrderService : IOrderService
    {
        // TODO : Add a new Order Table
        // Add a new Order Repository
        // Write a logic to insert data inside Order Table


        public Task<bool> CreateOrderAsync(CreateOrderDto orderDto)
        {
            throw new NotImplementedException();
        }
    }
}
