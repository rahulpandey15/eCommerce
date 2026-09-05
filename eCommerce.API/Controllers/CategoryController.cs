using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // TODO : OnlY admin can create category
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoryDto request)
        {
            var response
                  = await categoryService.AddCategoryAsync(request);

            if (response)
                return Created("Category Added", response);

            return BadRequest(response);
        }


    }
}
