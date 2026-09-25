using eCommerce.Application.Contracts;
using eCommerce.Application.DTO.Request;
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


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(
            RefreshTokenDto refreshTokenDto)
        {
            var tokenDetails
                 = await tokenService.RefreshTokenAsync(refreshTokenDto);

            return Ok(tokenDetails);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(
           RevokeTokenDto revokeToken)
        {
            var tokenDetails
                 = await tokenService.RevokeTokenAsync(revokeToken);

            return Ok(tokenDetails);
        }

    }
}
