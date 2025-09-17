using Asp.Versioning;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using MoviesAPIAdminModule;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor(); // Necessário para o LocalStorageService
//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions()); // Necessário para o bucket da Amazon
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movies API Admin Module",
        Version = "v1",
        Description = "API para administração de filmes, diretores e estúdios.",
        Contact = new OpenApiContact
        {
            Name = "Nathan Farias",
            Email = "francisco.nathan2@outlook.com",
            Url = new Uri("https://www.linkedin.com/in/nathan-farias-5bb97a24"),
        },
        License = new OpenApiLicense
        {
            Name = "Exemplo",
            Url = new Uri("https://github.com/N4thann"),
        }
    });

    // Habilitar no Swagger a autenticação com JWT na interface
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",//Nome do cabeçalho
        Type = SecuritySchemeType.ApiKey,//Chave de API a autenticação
        Scheme = "Bearer",//O schema especifíca o tipo de autenticação
        BearerFormat = "JWT",
        In = ParameterLocation.Header,//Indica que esse token virá no Header da requisição
        Description = "Bearer JWT ",
    });
    //Habilitar o requisito de segurança que as operações da API requerem o esquema de segurança Bearer
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Adicionando anotações da documentação
    c.EnableAnnotations();
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Versionamento automático na ASP.Net Core 
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;//Sem definir nenhum schema, ele utiliza por padrão o QueryString, lembrando que tamos o via URLSegment
    o.ApiVersionReader = ApiVersionReader.Combine(//Agora utiliizando as duas abordagens
                        new QueryStringApiVersionReader(),
                        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Logging.ClearProviders(); //Remove todos os providers de logging configurados por padrão pelo ASP.NET Core (como EventLog, Console, Debug)
builder.Logging.AddConsole();//Adiciona o provider que exibe logs no console/terminal da aplicação 
builder.Logging.AddDebug();//Adiciona o provider que exibe logs na janela de Debug Output do Visual Studio

// Chamadas para os métodos de extensão
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
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

app.UseCors();

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
