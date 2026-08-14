namespace eCommerce.Application.DTO.Response
{
    public record TokenResponseDto(
        string accessToken, string refreshToken);
}
