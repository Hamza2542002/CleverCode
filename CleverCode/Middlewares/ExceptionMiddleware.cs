using CleverCode.Helpers.Error_Response;
using System.Net;
using System.Text.Json;

namespace CleverCode.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment()
                    ? new ExceptionErrorResponse(ex.Message)
                    : new ExceptionErrorResponse(ex.Message, ex.StackTrace);


                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
