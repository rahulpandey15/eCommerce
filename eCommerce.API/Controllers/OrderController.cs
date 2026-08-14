using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {




        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderDto createOrderDto) {




            return Ok();
        }

        // two action
        // 1. create order (post)
        // 2. GetOrderDetail(GET)
    }
}
