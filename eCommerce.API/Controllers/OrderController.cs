using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using eCommerce.Application.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // will be only accessible to admin only 
    public class OrderController(IOrderService orderService) : ControllerBase
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
        public async Task<IActionResult> Post(
            CreateOrderDto createOrderDto,
            CancellationToken cancellationToken)
        {
            var result = await orderService.CreateOrderAsync(
                createOrderDto,
                HttpContext.TraceIdentifier,
                cancellationToken);

            return Accepted(new { Id = result.OrderId, result.OrderNumber });
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
