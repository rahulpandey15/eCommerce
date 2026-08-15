using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {


        [HttpGet]
  
        public async Task<IActionResult> Get()
        {
            var orderResponse
                 = new OrderResponse()
                 {
                     OrderName = "Shoes"
                 };
            return Ok(orderResponse);
        }



        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderDto createOrderDto) {

            return Ok();
        }

        // two action
        // 1. create order (post)
        // 2. GetOrderDetail(GET)
    }

    internal class OrderResponse
    {
        public string OrderName {  get; set; }
    }
}
