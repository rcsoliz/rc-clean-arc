using FluentValidation;
using Microsoft.IdentityModel.Tokens;    
using Serilog;
using System.Net;
using System.Text.Json;

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
                await HandleExceptionAsync(context, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title, errors) = exception switch
            {
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    "Error de validación",
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                ),

                UnauthorizedAccessException => (
                    HttpStatusCode.Unauthorized,
                    "No autorizado",
                    (Dictionary<string, string[]>?)null
                ),

                KeyNotFoundException => (
                    HttpStatusCode.NotFound,
                    "Recurso no encontrado",
                    (Dictionary<string, string[]>?)null
                ),

                SecurityTokenException => (
                  HttpStatusCode.Unauthorized,
                  "Token inválido o expirado",
                  (Dictionary<string, string[]>?)null
                ),

                _ => (
                  HttpStatusCode.InternalServerError,
                  "Error interno del servidor",
                  (Dictionary<string, string[]>?)null
                )
            };

            // Log solo errores reales — no validaciones ni 401
            if (statusCode == HttpStatusCode.InternalServerError)
                Log.Error(exception, "Error no controlado: {Message}", exception.Message);
            else
                Log.Warning(exception, "Error controlado [{StatusCode}]: {Message}",
                    (int)statusCode, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = (int)statusCode,
                title,
                errors,
                traceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
            
        }
    }
}
