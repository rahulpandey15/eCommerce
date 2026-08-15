using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }


        [HttpPost]
        public async Task<IActionResult> Post(
            ValidateUserDto validateUserDto)
        {
            var tokenDetails
                 = await tokenService.GetTokenAsync(validateUserDto);
            return Ok(tokenDetails);
        }

    }
}
