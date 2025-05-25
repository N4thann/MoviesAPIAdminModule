using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Application.DTOs.Response;

namespace MoviesAPIAdminModule.Extensions
{
    public static class ApiExceptionMiddleware
    {
        /// <summary>
        /// Este filtro, que implementa IExceptionFilter, é projetado para capturar e tratar exceções não tratadas que 
        /// ocorrem durante a execução de uma ação ou de outros filtros. Ele centraliza o tratamento de erros.Em vez de
        /// adicionar blocos try-catch em cada método de ação, este filtro intercepta automaticamente as exceções. 
        /// Isso segue o princípio DRY (Don't Repeat Yourself)
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            //Cria um contexto de reposta
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";//Tipo de conteúdo no formato Json

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetailsResponse()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace,
                        }.ToString());
                    }
                });
            });
        }
    }
}
