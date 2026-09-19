using eCommerce.Domain.DomainObjects;
using eCommerce.Infrastructure.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace eCommerce.Infrastructure.Persistence.Mappers
{
    public static class ProductRepositoryMappingExtension
    {
        public static Product ToProduct(this ProductDomain domain)
        {
            return new Product
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

        public static ProductDomain ToProductDomain(this Product entity)
        {
            return new ProductDomain
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity,
                CategoryId = entity.CategoryId,
                IsActive = entity.IsActive
            };
        }

        public static IEnumerable<ProductDomain> ToProductDomain(this IEnumerable<Product> products)
        {
            return products.Select(p => p.ToProductDomain());
        }
    }
}


