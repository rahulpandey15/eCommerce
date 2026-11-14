using eCommerce.Application.DTO.Request;
using eCommerce.Domain.DomainObjects;

namespace eCommerce.Application.Mappers
{
    public static class OrderServiceMappingExtension
    {
        public static CreateOrderDomain ToCreateOrderDomain(this CreateOrderDto dto)
        {
            return new CreateOrderDomain
            {
                UserId = dto.UserId,
                Items = dto.Items.Select(item => new CreateOrderItemDomain
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }
    }
}
