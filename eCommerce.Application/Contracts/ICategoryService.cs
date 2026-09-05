using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;

namespace eCommerce.Application.Contracts
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(CreateCategoryDto request);
        Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync();
        Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId);
    }
}
