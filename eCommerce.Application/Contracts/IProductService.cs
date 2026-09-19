using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using System.Collections.Generic;

namespace eCommerce.Application.Contracts
{
    public interface IProductService
    {
        Task<int> CreateProductAsync(CreateProductDto dto);
        Task<IEnumerable<ProductResponseDto>> GetProductsAsync();
        Task<ProductResponseDto> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(UpdateProductDto dto);
        Task<(bool Success, bool HasOrders, bool Exists)> DeleteProductAsync(int id);
    }
}

