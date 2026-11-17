using eCommerce.Application.Common.Messaging;
using eCommerce.Application.Common.Messaging.Events;
using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;


namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IOutboxWriter _outboxWriter;

        public OrderRepository(AppDbContext dbContext,
            IOutboxWriter outboxWriter)
        {
            _dbContext = dbContext;
            _outboxWriter = outboxWriter;
        }


        public async Task<OrderCreationResult> CreateAsync(
            CreateOrderDomain orderDto, 
            string? correlationId, 
            CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // 1. Inserting a record in my Order Tables
            var order = new Order
            {
                OrderNumber = $"ORD-{DateTime.UtcNow.Ticks}",
                TotalAmount = orderDto.Items.Sum(i => i.UnitPrice * i.Quantity),
                Status = "Pending",
                UserId = orderDto.UserId,
                CreatedOn = System.DateTime.UtcNow,
                CreatedBy = "User",
                OrderItems = orderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.UnitPrice * i.Quantity,
                    CreatedOn = System.DateTime.UtcNow,
                    CreatedBy = "User",
                }).ToList()
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //2. Insert a record in my Outbox Table

            var integrationEvent = new OrderCreatedEvent
            {
                CorrelationId = correlationId,
                Payload = new OrderCreatedPayload
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Items = order.OrderItems.Select(item => new OrderCreatedItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToArray()
                }
            };

            await _outboxWriter.AddAsync(integrationEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new OrderCreationResult(order.Id, order.OrderNumber);
        }
    }
}
