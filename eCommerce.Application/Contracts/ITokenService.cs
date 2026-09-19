using eCommerce.Application.DTO.Request;
using eCommerce.Application.DTO.Response;

namespace eCommerce.Application.Contracts
{
    public interface ITokenService
    {
      
        Task<TokenResponseDto> GetTokenAsync(
            ValidateUserDto validateUserDto);

        Task<TokenResponseDto> RefreshTokenAsync(
          RefreshTokenDto validateUserDto);

    }
}
