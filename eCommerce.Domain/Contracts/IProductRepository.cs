using eCommerce.Domain.DomainObjects;
using System.Collections.Generic;

namespace eCommerce.Domain.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDomain>> GetProductsAsync();
        Task<ProductDomain> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(ProductDomain request);
        Task<bool> UpdateProductAsync(ProductDomain request);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> IsDuplicateProductAsync(string name, int? excludingId = null);
        Task<bool> HasOrderItemsAsync(int productId);
    }
}

