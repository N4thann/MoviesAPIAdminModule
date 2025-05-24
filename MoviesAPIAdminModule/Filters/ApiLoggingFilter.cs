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
            //executa antes do método Action
            _logger.LogInformation("### Executado -> OnActionExecuted");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("####################################");
        }
    }
}
