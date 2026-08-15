using eCommerce.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eCommerce.Domain.Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDomain>> GetCategoriesAsync();

        Task<CategoryDomain> GetCategoryByIdAsync(int id);

        Task<bool> AddCategoryAsync(CategoryDomain request);


    }
}
