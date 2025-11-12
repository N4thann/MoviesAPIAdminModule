using Asp.Versioning;
using Microsoft.Extensions.FileProviders;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor(); // Necessário para o LocalStorageService
//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions()); // Necessário para o bucket da Amazon
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.PostProcess = document =>
    {
        document.Info.Title = "Movies API Admin Module";
        document.Info.Version = "v1"; // Será sobrescrito pelo versionamento
        document.Info.Description = "API para administração de filmes, diretores e estúdios.";

        document.Info.Contact = new NSwag.OpenApiContact
        {
            Name = "Nathan Farias",
            Email = "francisco.nathan2@outlook.com",
            Url = "https://www.linkedin.com/in/nathan-farias-5bb97a24"
        };

        document.Info.License = new NSwag.OpenApiLicense
        {
            Name = "Exemplo",
            Url = "https://github.com/N4thann"
        };
    };

    // 2. Configuração de Segurança JWT (Bearer)
    // (Isso substitui o AddSecurityDefinition)
    settings.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Insira o token JWT: Bearer {seu_token}",
    });

    // 3. Aplica o "cadeado" de segurança a todos os endpoints
    // (Isso substitui o AddSecurityRequirement)
    settings.OperationProcessors.Add(
        new OperationSecurityScopeProcessor("Bearer"));
});

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader(),
                        new UrlSegmentApiVersionReader()
    );
});

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Logging.ClearProviders(); //Remove todos os providers de logging configurados por padrão pelo ASP.NET Core (como EventLog, Console, Debug)
builder.Logging.AddConsole();//Adiciona o provider que exibe logs no console/terminal da aplicação 
builder.Logging.AddDebug();//Adiciona o provider que exibe logs na janela de Debug Output do Visual Studio

// Chamadas para os métodos de extensão
// 1. Registra os serviços de infraestrutura primeiro (incluindo o AddIdentity)
builder.Services.AddInfrastructureServices(builder.Configuration);
// 2. Registra os serviços da aplicação
builder.Services.AddApplicationServices();
// 3. Registra os serviços da Web API por último, para que a configuração de JWT
//    sobrescreva os padrões do Identity.
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureExceptionHandler();
    // 1. O Gerador (onde o arquivo .json é criado)
    // Precisamos dizer a ele para usar o MESMO caminho que a UI espera.
    app.UseOpenApi(settings =>
    {
        settings.Path = "/openapi/{documentName}/openapi.json";
    });

    // 2. A UI (onde o usuário vê)
    // (O seu já estava quase certo, apontando para o caminho correto)
    app.UseSwaggerUi(settings =>
    {
        // Este caminho DEVE ser o mesmo do 'settings.Path' acima
        settings.DocumentPath = "/openapi/{documentName}/openapi.json";
        settings.DocumentTitle = "Movies API - Docs";
    });
}

// ===== CONFIGURAÇÃO CORRIGIDA DE ARQUIVOS ESTÁTICOS =====

// 1. Pega o caminho relativo do appsettings.json
var staticFilesPath = builder.Configuration.GetValue<string>("FileStorageSettings:LocalUploadPath");

if (string.IsNullOrEmpty(staticFilesPath))
{
    throw new InvalidOperationException("A chave 'FileStorageSettings:LocalUploadPath' não está configurada no appsettings.json.");
}

// 2. Monta o caminho físico completo usando a raiz do projeto (não a pasta bin)
var physicalPath = Path.Combine(builder.Environment.ContentRootPath, staticFilesPath);

// 3. Garante que o diretório exista no disco
if (!Directory.Exists(physicalPath))
{
    Directory.CreateDirectory(physicalPath);
}

// 4. Configura o middleware para servir arquivos da pasta correta
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(physicalPath),
    RequestPath = $"/{staticFilesPath.Replace("\\", "/")}"
});

app.UseHttpsRedirection(); // Recomendado adicionar para produção
app.UseStaticFiles(); // Para servir arquivos (como imagens de filmes)

app.UseRouting(); // Embora implícito, é bom saber que está aqui
app.UseRateLimiter();

app.UseCors("AllowMyClient");

app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

app.Run();

/*
 * 
 * Diferenças entre Manipulador JWT Tradicional (AddJwtBearer) e Novo Manipulador Identity (AddBearerToken)
 -A abordagem manual (.AddJwtBearer) sempre será necessária para cenários avançados onde a flexibilidade é mais 
importante que a simplicidade. / JWT (RFC 7519) / Total. Você controla cada parâmetro de validação / Config: Manual e explícita (você define todas as regras)
/	Casos de Uso: APIs complexas, microserviços, alta customização / Sua responsabilidade total (armazenar, validar, revogar)

-A abordagem automática (.AddBearerToken) será a escolha de 90% dos novos projetos, especialmente para desenvolvedores que 
estão começando ou para aplicações onde os padrões de segurança da Microsoft são perfeitamente adequados (a grande maioria dos casos).
/ Também é JWT, mas gerenciado pelo Identity / Pode criar os seus ou usar os pré-fabricados com .MapIdentityApi() / Limitado. A Microsoft abstrai os detalhes de você
/ Complexidade Muito Baixa. Ideal para desenvolvimento rápido / Casos de uso: SPAs, aplicativos móveis, projetos onde os padrões são suficientes

 */
