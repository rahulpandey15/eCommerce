using eCommerce.Application.DTO.Request;
using eCommerce.Domain.DomainObjects;

namespace eCommerce.Application.Mappers
{
    public static class ProductServiceMappingExtension
    {
        public static ProductDomain ToProductDomain(this CreateProductDto dto)
        {
            return new ProductDomain
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive
            };
        }

        public static ProductDomain ToProductDomain(this UpdateProductDto dto)
        {
            return new ProductDomain
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive
            };
        }
    }
}

