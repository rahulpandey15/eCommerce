using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;

namespace eCommerce.Application.Contracts
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(CreateCategoryDto request);
        Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync();
        Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId);
        Task<(bool Success, bool HasProducts, bool Exists)> DeleteCategoryAsync(int categoryId);
    }
}
