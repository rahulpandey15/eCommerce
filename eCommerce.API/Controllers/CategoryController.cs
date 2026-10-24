using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
        ICategoryService categoryService,
        AppDbContext appDbContext,
        ILogger<CategoryController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Processing GET endpoint in Category Controller");

            var categories = await categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoryDto request)
        {
            var createdId = await categoryService.AddCategoryAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = createdId }, null);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await categoryService.DeleteCategoryAsync(id);

            if (!result.Exists)
                return NotFound();

            if (result.HasProducts)
                return Conflict(new { message = "Category has products and cannot be deleted." });

            if (result.Success)
                return NoContent();

            return BadRequest();
        }


        [HttpGet("product-count-by-categories")]
        public async Task<IActionResult> CategoriesWithProductCount()
        {
            var data = await appDbContext.Categories
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductCount = x.Products.Count,
                    ActiveProducts = x.Products.Count(s => s.IsActive),
                    InactiveProduct = x.Products.Count(s => !s.IsActive)
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("top-categories-by-revenue")]
        public async Task<IActionResult> TopCategoriesByRevenue()
        {
            // Order Item,. product, Category

            var data = await appDbContext.OrderItems
                       .Include(oi => oi.Product)
                       .Where(oi => oi.Product != null)
                       .GroupBy(oi => new
                       {
                           CategoryId = 
                                oi.Product.CategoryId,
                           CategoryName = 
                            oi.Product.Category != null ? oi.Product.Category.Name : null
                       })
                       .Select(f => new
                       {
                           CategoryId = f.Key.CategoryId,
                           CategoryName = f.Key.CategoryName,
                           Revenue = f.Sum(x => x.TotalPrice),
                           UnitSold = f.Sum(x => x.Quantity)
                       })
                       .OrderByDescending(x => x.Revenue)
                       .ToListAsync();
            return Ok(data);
        }
    }
}
