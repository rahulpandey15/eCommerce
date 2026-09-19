using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;
using eCommerce.Application.Mappers;
using eCommerce.Domain.Contracts;

namespace eCommerce.Application.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<int> AddCategoryAsync(CreateCategoryDto request)
        {

            //DTO-- domain
            var categoryDomain
                 = request.ToCategoryDomain();

            return await categoryRepository.AddCategoryAsync(
                categoryDomain);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync()
        {
            var domains = await categoryRepository.GetCategoriesAsync();

            return domains.Select(d => new CategoryResponseDto()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            });
        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId)
        {
            var domain = await categoryRepository.GetCategoryByIdAsync(categoryId);

            if (domain is null)
                return null;

            return new CategoryResponseDto()
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description
            };
        }

        public async Task<(bool Success, bool HasProducts, bool Exists)> DeleteCategoryAsync(int categoryId)
        {
            var exists = await categoryRepository.GetCategoryByIdAsync(categoryId) != null;
            if (!exists)
                return (false, false, false);

            var hasProducts = await categoryRepository.HasProductsAsync(categoryId);
            if (hasProducts)
                return (false, true, true);

            var deleted = await categoryRepository.DeleteCategoryAsync(categoryId);
            return (deleted, false, true);
        }
    }
}
