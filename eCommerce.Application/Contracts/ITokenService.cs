using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;

namespace eCommerce.Application.Contracts
{
    public interface ITokenService
    {
        // Create a method, that will accept username/password
        // and it will return me a access token and refresh token ---
        Task<TokenResponseDto> GetTokenAsync(
            ValidateUserDto validateUserDto);

    }
}
