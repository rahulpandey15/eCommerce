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


        public async Task<int> AddCategoryAsync(CategoryDomain request)
        {
            //Domain-- Entities
            var category
                 = request.ToCategory();

            category.CreatedBy = "system";
            category.CreatedOn = DateTime.Now;

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            // EF populates the Id after SaveChanges
            return category.Id;
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

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _dbContext.Products.AnyAsync(p => p.CategoryId == categoryId);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
                return false;

            _dbContext.Categories.Remove(category);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }
    }
}
