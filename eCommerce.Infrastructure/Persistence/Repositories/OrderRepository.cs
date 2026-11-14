using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;


namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        public Task<OrderCreationResult> CreateAsync(
            CreateOrderDomain orderDto, 
            string orderNumber, 
            string? correlationId, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
