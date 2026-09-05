using eCommerce.Application.DTO.Request;
using eCommerce.Domain.DomainObjects;


namespace eCommerce.Application.Mappers
{
    public static class CategoryServiceMappingExtension
    {

        public static CategoryDomain ToCategoryDomain(
            this CreateCategoryDto categoryDto)
        {
            return new CategoryDomain()
            {
                Description = categoryDto.Description,
                Name = categoryDto.Name,
            };
        }

    }
}
