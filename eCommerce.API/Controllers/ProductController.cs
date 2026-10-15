using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController(
        IProductService productService,
        AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductDto request)
        {
            var createdId = await productService.CreateProductAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateProductDto request)
        {
            if (id != request.Id) return BadRequest();

            var updated = await productService.UpdateProductAsync(request);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await productService.DeleteProductAsync(id);

            if (!result.Exists) return NotFound();
            
            if (result.HasOrders) return Conflict(new { message = "Product is referenced by orders and cannot be deleted." });
            
            if (result.Success) return NoContent();
            
            return BadRequest();
        }

        [HttpGet("top-prices")]
        public async Task<IActionResult> TopPrices(int take = 5)
        {
            var items
                 = await dbContext.Products
                 .Include(p => p.Category)
                 .OrderByDescending(x => x.Price)
                 .Take(take)
                 .Select(x => new
                 {
                     x.Id,
                     x.Name, 
                     x.Description, x.Price,
                     Cateofory = x.Category
                 }).ToListAsync();
            return Ok(items);
        }

        [HttpGet("stock-summary")]
        public async Task<IActionResult> StockSummary()
        {
            var summary = await dbContext.Products
                .GroupBy(x => new { x.CategoryId, x.Category.Name })
                .Select(g => new
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    Count = g.Count(),
                    TotalStock = g.Sum(x => x.StockQuantity),
                    AvgPrice = g.Average(x => x.Price)
                }).ToListAsync();


            return Ok(summary);
                
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string searchText="", int page=1, int pazeSize = 10)
        {
            var query = dbContext.Products.AsQueryable(); // 

            if(!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerm
                     = searchText.Trim().ToLower();

                query = query.Where(x => x.Name.ToLower().Contains(searchTerm));
            }

            var total = await query.CountAsync();

            var items
                 = await query.OrderBy(x => x.Name)
                 .Skip((page - 1) * pazeSize)
                 .Take(page)
                 .Select(x => new
                 {
                     Id = x.CategoryId,
                     Name = x.Name,
                 })
                 .ToListAsync();

            return Ok(items);
        }
    }
}

