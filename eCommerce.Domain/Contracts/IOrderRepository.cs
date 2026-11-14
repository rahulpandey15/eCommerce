using eCommerce.Domain.DomainObjects;


namespace eCommerce.Domain.Contracts
{
    public interface IOrderRepository
    {
     Task<OrderCreationResult> CreateAsync(
           CreateOrderDomain orderDto,
             string orderNumber,
             string? correlationId,
             CancellationToken cancellationToken = default);
    }
}
