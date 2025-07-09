
using Domain.SeedWork.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPIAdminModule.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        /// <summary>
        /// Atua em um nível mais granular, tratando exceções específicas das ações do controlador. 
        /// Ele padroniza as respostas de erro para o cliente quando uma exceção ocorre dentro de uma ação MVC.
        /// </summary>
        /// <param name="logger"></param>
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) => _logger = logger;

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu uma exceção tratada pelo filtro de API.");

            if (context.Exception is KeyNotFoundException keyNotFoundException)
            {
                context.Result = new NotFoundObjectResult(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    typeException = (nameof(KeyNotFoundException)),
                    message = keyNotFoundException.Message 
                });
                context.ExceptionHandled = true;
                return;
            }
            else if (context.Exception is ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    statusCode = StatusCodes.Status406NotAcceptable,
                    typeException = (nameof(ValidationException)),
                    message = validationException.Message
                });
                context.ExceptionHandled = true;
                return;
            }
            // 3. Tratamento para "Operação Inválida" (erros de negócio que não são de validação de entrada, mas sim de estado)
            // Lembre-se que você lançou InvalidOperationException no UseCase para erros inesperados.
            // Se você quiser diferenciar erros de validação de entrada (400) de outros erros de lógica de negócio (400 ou 409),
            // considere criar uma exceção de domínio mais específica ou ajustar a mensagem da InvalidOperationException.
            else if (context.Exception is InvalidOperationException invalidOperationException)
            {
                _logger.LogWarning(invalidOperationException, "Operação de negócio inválida detectada.");
                context.Result = new BadRequestObjectResult(new 
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    typeException = (nameof(InvalidOperationException)),
                    message = invalidOperationException.Message
                });
                context.ExceptionHandled = true;
                return;
            }

            // Se a exceção não for tratada por nenhum dos 'if/else if' acima,
            // ela não terá 'context.Result' definido e 'context.ExceptionHandled' será false.
            // Isso fará com que a exceção continue propagando na pipeline
            // até ser capturada pelo seu middleware global (ApiExceptionMiddleware),
            // que a transformará em um 500 Internal Server Error.
        }
    }
}
/*O IExceptionFilter permite um tratamento de exceções mais "refinado" e "semântico" dentro da 
 * lógica de negócios e validação da sua API. Você pode usar os filtros para traduzir exceções de sua aplicação em 
 * códigos de status HTTP e mensagens de erro que fazem mais sentido para o cliente da API 
 * (por exemplo, "recurso não encontrado", "dados inválidos", "usuário não autorizado").
 */
