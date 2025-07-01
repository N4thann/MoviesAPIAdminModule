using MoviesAPIAdminModule;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>{
    options.Filters.Add(typeof(ApiExceptionFilter));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Esta linha � crucial para que o Swagger UI saiba qual vers�o da API ele deve exibir
    // e para satisfazer a valida��o de "valid version field".
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Movies API Admin Module", // T�tulo da sua API
        Version = "v1", // A vers�o da sua API, que resolve o erro "valid version field"
        Description = "API para administra��o de filmes, diretores e est�dios.", // Descri��o opcional
        // Outras propriedades como Contact, License, etc., s�o opcionais
    });

    // Certifique-se de que esta linha esteja presente para habilitar os atributos [SwaggerOperation]
    c.EnableAnnotations();

    // Verifique se a configura��o para incluir os XML Comments tamb�m est� l� e correta
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Logging.ClearProviders(); //Remove todos os providers de logging configurados por padr�o pelo ASP.NET Core (como EventLog, Console, Debug)
builder.Logging.AddConsole();//Adiciona o provider que exibe logs no console/terminal da aplica��o 
builder.Logging.AddDebug();//Adiciona o provider que exibe logs na janela de Debug Output do Visual Studio

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddScoped<ApiLoggingFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
