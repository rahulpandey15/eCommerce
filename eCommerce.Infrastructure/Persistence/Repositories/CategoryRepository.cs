using eCommerce.Domain.Contracts;
using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AddCategoryAsync(CategoryDomain request)
        {
            //Domain-- Entities
            var category
                 = request.ToCategory();

            category.CreatedBy = "system";
            category.CreatedOn = DateTime.Now;

            _dbContext.Categories.Add(category);
            int rowsInserted = await _dbContext.SaveChangesAsync();

            return rowsInserted > 0;
        }

        public async Task<IEnumerable<CategoryDomain>> GetCategoriesAsync()
        {
            var categories
                 = await _dbContext.Categories.ToListAsync();

            return categories.ToCategoryDomain();


        }

        public async Task<CategoryDomain> GetCategoryByIdAsync(int id)
        {
            var categories
                 = await _dbContext.Categories.FindAsync(id);

            //Domain-- Entities
            return categories.ToCategoryDomain();
        }
    }
}
