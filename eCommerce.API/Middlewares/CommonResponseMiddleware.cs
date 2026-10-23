using eCommerce.Application.DTO.Response;
using System.Text.Json;

namespace eCommerce.API.Middlewares
{
    public class CommonResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CommonResponseMiddleware> _logger;

        public CommonResponseMiddleware(
            RequestDelegate next,
            ILogger<CommonResponseMiddleware> logger)
        {
            _next = next;
            this._logger = logger;
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
                _logger.LogError($"Error Occured {ex.Message}");
                throw;
            }

            if (context.Response.ContentType is not null
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
            else
            {
                // For non-JSON responses (e.g. HTML from Hangfire dashboard), copy the buffered
                // response back to the original response stream so the client receives it.
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
            }
        }
    }
}
