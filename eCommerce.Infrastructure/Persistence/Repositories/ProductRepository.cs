using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddProductAsync(ProductDomain request)
        {
            var product = request.ToProduct();
            product.CreatedOn = DateTime.Now;
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product is null) return false;
            _dbContext.Products.Remove(product);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<ProductDomain> GetProductByIdAsync(int id)
        {
            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return null;
            return product.ToProductDomain();
        }

        public async Task<IEnumerable<ProductDomain>> GetProductsAsync()
        {
            var products = await _dbContext.Products.AsNoTracking().ToListAsync();
            return products.ToProductDomain();
        }

        public async Task<bool> HasOrderItemsAsync(int productId)
        {
            return await _dbContext.OrderItems.AnyAsync(oi => oi.ProductId == productId);
        }

        public async Task<bool> IsDuplicateProductAsync(string name, int? excludingId = null)
        {
            if (excludingId.HasValue)
                return await _dbContext.Products.AnyAsync(p => p.Name == name && p.Id != excludingId.Value);

            return await _dbContext.Products.AnyAsync(p => p.Name == name);
        }

        public async Task<bool> UpdateProductAsync(ProductDomain request)
        {
            var existing = await _dbContext.Products.FindAsync(request.Id);
            if (existing is null) return false;

            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.Price = request.Price;
            existing.StockQuantity = request.StockQuantity;
            existing.CategoryId = request.CategoryId;
            existing.IsActive = request.IsActive;
            existing.ModifiedOn = DateTime.Now;

            _dbContext.Products.Update(existing);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }
    }
}


