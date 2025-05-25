using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPIAdminModule.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) => _logger = logger;

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            //executa antes do método Action
            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString}");
            _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("####################################");
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            //executa depois do método Action
            _logger.LogInformation("### Executado -> OnActionExecuted");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("####################################");
        }
    }
}
/*Existem 5 tipos de filtros na asp.net core, authorization, resource, action, exception e result.
 * São 2 abordagens, implementação síncrona e assíncrona. 
 * Síncronos executam código antes e depois do estágio do pipeline definidos pelo os métodos
 * OnActionExecuting e OnActionExecuted. Os filtros podem ser adicionados eo pipeline em um dos 
 * tres escopos: Pelo método Action, pela a classe controlador ou Globalmente(é aplicado a todos os controladores e actions)
 */
