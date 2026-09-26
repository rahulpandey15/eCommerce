using eCommerce.Application.DTO.Response;
using System.Text.Json;

namespace eCommerce.API.Middlewares
{
    public class CommonResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CommonResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var memoryStream
                 = new MemoryStream();


            context.Response.Body = memoryStream;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                throw;
            }

            if(context.Response.ContentType is not null 
                && context.Response.ContentType.Contains("application/json"))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);

                var responseBody = 
                    await new StreamReader(memoryStream).ReadToEndAsync(); // line of code will have real response from controller


                var repsonseObj
                     = new ApiResposeDto<object>(
                         success: context.Response.StatusCode is >= 200 and <= 299,
                         message: "Request completed successfully",
                         data: JsonSerializer.Deserialize<object>(responseBody)!
                         );


                var jsonResponse
                     = JsonSerializer.Serialize(repsonseObj);

                context.Response.Body = originalBody;

                await context.Response.WriteAsync(jsonResponse);

            }
        }
    }
}
