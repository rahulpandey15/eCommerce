using eCommerce.Application.DTO.Request;

namespace eCommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(CreateOrderDto orderDto);
    }
}
