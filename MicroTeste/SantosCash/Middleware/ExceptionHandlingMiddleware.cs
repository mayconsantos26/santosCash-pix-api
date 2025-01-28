using System.Net;
using System.ComponentModel.DataAnnotations;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) // Para tratamentos de erros sensíveis
    {
        try
        {
            //Chama o próximo middleware na cadeia
            await _next(context);
        }
        catch (ValidationException valEx)
        {
            // Log de exceção de validação
            _logger.LogError(valEx, "Falha na validação dos dados.");

            // Resposta específica para falha de validação
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "applicationjson";

            var response = new
            {
                message = "Campos inválidos ou dados incompletos.",
                errors = valEx.ValidationResult.ErrorMessage // Mensagem de erro da validação
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
        catch (ArgumentException argEx)
        {
            // Log de exceção de argumento inválido
            _logger.LogError(argEx, "Argumento inválido.");

            // Resposta para argumentos inválidos
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "applicationjson";

            var response = new
            {
                message = "Um ou mais parâmetros fornecidos são inválidos.",
                details = argEx.Message
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            // Log de erro não tratado
            _logger.LogError(ex, "Erro não tratado.");

            // Resposta genérica para erro interno
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "applicationjson";

            var response = new
            {
                message = "Ocorreu um erro inesperado. Tente novamente mais tarde."
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
 }
