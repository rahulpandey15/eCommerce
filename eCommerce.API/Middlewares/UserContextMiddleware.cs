using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eCommerce.API.Middlewares
{
    public class UserContextMiddleware
    {

        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = await context.GetTokenAsync("access_token");

            if (string.IsNullOrWhiteSpace(token))
                await _next(context);
            else
            {
                string currentUserName = GetUserName(token);
                context.Request.Headers.Add("LoggedInUserName", currentUserName);
                await _next(context);
            }
        }

        private string GetUserName(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var currentUserName =
                jwtToken.Claims.FirstOrDefault(
                    x => x.Type == ClaimTypes.Email)
                .Value;

            return currentUserName;
        }
    }
}
