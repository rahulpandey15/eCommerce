using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController(IProductService productService) : ControllerBase
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
    }
}

