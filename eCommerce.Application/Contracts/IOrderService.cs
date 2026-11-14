using eCommerce.Application.DTO.Request;

namespace eCommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<OrderCreationResultDto> CreateOrderAsync(CreateOrderDto orderDto, string? correlationId, CancellationToken cancellationToken = default);
    }
}
