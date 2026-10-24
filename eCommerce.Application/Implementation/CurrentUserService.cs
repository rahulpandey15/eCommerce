using eCommerce.Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace eCommerce.Application.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }


        public string GetCurrentUser()
        {
            var loggerInUserEmail
                 = _httpContextAccessor.HttpContext.Request.Headers["LoggedInUserName"];

            if (!string.IsNullOrWhiteSpace(loggerInUserEmail))
                return loggerInUserEmail!;


            throw new Exception("No User Context Available");
        }
    }
}
