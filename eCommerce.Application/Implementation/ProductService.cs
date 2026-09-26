using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using eCommerce.Application.Mappers;
using eCommerce.Domain.Contracts;

namespace eCommerce.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public ProductService(
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            this._currentUserService = currentUserService;
        }

        public async Task<int> CreateProductAsync(CreateProductDto dto)
        {
            // basic validation
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required", nameof(dto.Name));

            if (dto.Price < 0)
                throw new ArgumentException("Price must be >= 0", nameof(dto.Price));

            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist");

            // duplicate check
            if (await _productRepository.IsDuplicateProductAsync(dto.Name))
                throw new InvalidOperationException("A product with the same name already exists.");

            var domain = dto.ToProductDomain();
            domain.CreatedBy = _currentUserService.GetCurrentUser();

            return await _productRepository.AddProductAsync(domain);
        }

        public async Task<(bool Success, bool HasOrders, bool Exists)> DeleteProductAsync(int id)
        {
            var exists = await _productRepository.GetProductByIdAsync(id) != null;
            if (!exists) return (false, false, false);

            var hasOrders = await _productRepository.HasOrderItemsAsync(id);
            if (hasOrders) return (false, true, true);

            var deleted = await _productRepository.DeleteProductAsync(id);
            return (deleted, false, true);
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var domain = await _productRepository.GetProductByIdAsync(id);
            if (domain == null) return null;

            return new ProductResponseDto
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description,
                Price = domain.Price,
                StockQuantity = domain.StockQuantity,
                CategoryId = domain.CategoryId,
                IsActive = domain.IsActive
            };
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsAsync()
        {
            var domains = await _productRepository.GetProductsAsync();
            return domains.Select(d => new ProductResponseDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                StockQuantity = d.StockQuantity,
                CategoryId = d.CategoryId,
                IsActive = d.IsActive
            });
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
        {
            if (dto.Price < 0) throw new ArgumentException("Price must be >= 0", nameof(dto.Price));

            var existing = await _productRepository.GetProductByIdAsync(dto.Id);
            if (existing == null) return false;

            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null) throw new InvalidOperationException("Category does not exist");

            if (await _productRepository.IsDuplicateProductAsync(dto.Name, dto.Id))
                throw new InvalidOperationException("A product with the same name already exists.");

            var domain = dto.ToProductDomain();
            domain.CreatedBy = _currentUserService.GetCurrentUser();
            return await _productRepository.UpdateProductAsync(domain);
        }
    }
}

