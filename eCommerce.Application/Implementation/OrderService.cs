using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.Mappers;
using eCommerce.Domain.Contracts;


namespace eCommerce.Application.Implementation
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<OrderCreationResultDto> CreateOrderAsync(
            CreateOrderDto orderDto, 
            string? correlationId, 
            CancellationToken cancellationToken = default)
        {
            var domain = orderDto.ToCreateOrderDomain();

            var response = 
                await orderRepository.CreateAsync(domain, correlationId, cancellationToken);

            return new OrderCreationResultDto(response.OrderId, response.OrderNumber);
        }
    }
}
