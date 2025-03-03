using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace nx_api.WebApi.MiddlewareExceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Chama o próximo middleware no pipeline
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Se ocorrer uma exceção, chama o método HandleException
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Loga a exceção para rastrear o erro
            _logger.LogError(exception, "Ocorreu um erro inesperado.");

            // Configura o status code para "InternalServerError" por padrão
            context.Response.ContentType = "application/json";

            // Se a exceção for do tipo CustomException, trata ela de forma personalizada
            if (exception is CustomException customEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400 para erro do cliente
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = customEx.Message,  // Mensagem personalizada da exceção
                    StackTrace = exception.StackTrace  // Stack trace para depuração
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                // Para exceções gerais, retorna um erro genérico
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    StackTrace = exception.StackTrace
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

}
