using System.Net;
using System.Text.Json;

namespace LowkeysDrops.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió una excepción no manejada.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Error interno del servidor.";

            if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is InvalidOperationException || exception is ArgumentException || exception.Message.StartsWith("CONFLICT:"))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message.Replace("CONFLICT:", "");
            }
            // EF Core DB update exceptions often wrap SQL exceptions
            else if (exception is Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                statusCode = (int)HttpStatusCode.Conflict;
                var innerMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                if (innerMsg.Contains("CK_") || innerMsg.Contains("conflicted with the CHECK constraint"))
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Los datos proporcionados no cumplen con las reglas de validación del sistema.";
                }
                else if (innerMsg.Contains("Cannot insert duplicate key") || innerMsg.Contains("Violation of UNIQUE KEY constraint"))
                {
                    message = "El registro ya existe o hay un conflicto de unicidad.";
                }
                else
                {
                    message = "Ocurrió un conflicto al guardar los cambios en la base de datos.";
                }
            }
            else if (exception is Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                if (sqlEx.Number >= 50000)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = sqlEx.Message;
                }
                else if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                {
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = "El registro ya existe o hay un conflicto de unicidad.";
                }
                else if (sqlEx.Number == 547)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Los datos proporcionados no cumplen con las reglas de validación (Check constraint).";
                }
                else
                {
                    message = "Ocurrió un error en la base de datos.";
                }
            }

            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                status = statusCode,
                message = message
            });

            return context.Response.WriteAsync(result);
        }
    }
}
