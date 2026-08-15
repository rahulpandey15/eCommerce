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

        public async Task<bool> AddCategoryAsync(CreateCategoryDto request)
        {

            //DTO-- domain
            var categoryDomain
                 = request.ToCategoryDomain();


            return await categoryRepository.AddCategoryAsync(
                categoryDomain);
        }

        public Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync()
        {

            //DTO-- domain
            throw new NotImplementedException();
        }

        public Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId)
        {
            //DTO-- domain
            throw new NotImplementedException();
        }
    }
}
