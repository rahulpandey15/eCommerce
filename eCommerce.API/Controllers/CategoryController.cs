using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
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


    }
}
