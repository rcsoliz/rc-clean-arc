using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;   

namespace Presentation.Middleware
{
    public class ErrorHandlingMiddleware
    {
        public readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
            }
        }
    }
}
