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
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: Status Code 500");

            context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação: Status Code 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
/*O IExceptionFilter permite um tratamento de exceções mais "refinado" e "semântico" dentro da 
 * lógica de negócios e validação da sua API. Você pode usar os filtros para traduzir exceções de sua aplicação em 
 * códigos de status HTTP e mensagens de erro que fazem mais sentido para o cliente da API 
 * (por exemplo, "recurso não encontrado", "dados inválidos", "usuário não autorizado").
 */
