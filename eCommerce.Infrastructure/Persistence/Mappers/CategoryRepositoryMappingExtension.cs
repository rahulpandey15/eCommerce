using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;


namespace eCommerce.Infrastructure.Persistence.Mappers
{
    public static class CategoryRepositoryMappingExtension
    {

        public static Category ToCategory(this CategoryDomain domain)
        {
            return new Category()
            {
                Name = domain.Name,
                Description = domain.Description,
            };
        }


        public static IEnumerable<CategoryDomain> ToCategoryDomain(
            this IEnumerable<Category> categories)
        {
            return categories.Select(x => x.ToCategoryDomain());
        }

        public static CategoryDomain ToCategoryDomain(this Category domain)
        {
            return new CategoryDomain()
            {
                Name = domain.Name,
                Description = domain.Description,
            };
        }

    }
}
