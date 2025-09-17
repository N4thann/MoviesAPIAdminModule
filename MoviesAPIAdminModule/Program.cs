using Asp.Versioning;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using MoviesAPIAdminModule;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor(); // Necess�rio para o LocalStorageService
//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions()); // Necess�rio para o bucket da Amazon
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movies API Admin Module",
        Version = "v1",
        Description = "API para administra��o de filmes, diretores e est�dios.",
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

    // Habilitar no Swagger a autentica��o com JWT na interface
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",//Nome do cabe�alho
        Type = SecuritySchemeType.ApiKey,//Chave de API a autentica��o
        Scheme = "Bearer",//O schema especif�ca o tipo de autentica��o
        BearerFormat = "JWT",
        In = ParameterLocation.Header,//Indica que esse token vir� no Header da requisi��o
        Description = "Bearer JWT ",
    });
    //Habilitar o requisito de seguran�a que as opera��es da API requerem o esquema de seguran�a Bearer
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

    // Adicionando anota��es da documenta��o
    c.EnableAnnotations();
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Versionamento autom�tico na ASP.Net Core 
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;//Sem definir nenhum schema, ele utiliza por padr�o o QueryString, lembrando que tamos o via URLSegment
    o.ApiVersionReader = ApiVersionReader.Combine(//Agora utiliizando as duas abordagens
                        new QueryStringApiVersionReader(),
                        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Logging.ClearProviders(); //Remove todos os providers de logging configurados por padr�o pelo ASP.NET Core (como EventLog, Console, Debug)
builder.Logging.AddConsole();//Adiciona o provider que exibe logs no console/terminal da aplica��o 
builder.Logging.AddDebug();//Adiciona o provider que exibe logs na janela de Debug Output do Visual Studio

// Chamadas para os m�todos de extens�o
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

// ===== CONFIGURA��O CORRIGIDA DE ARQUIVOS EST�TICOS =====

// 1. Pega o caminho relativo do appsettings.json
var staticFilesPath = builder.Configuration.GetValue<string>("FileStorageSettings:LocalUploadPath");

if (string.IsNullOrEmpty(staticFilesPath))
{
    throw new InvalidOperationException("A chave 'FileStorageSettings:LocalUploadPath' n�o est� configurada no appsettings.json.");
}

// 2. Monta o caminho f�sico completo usando a raiz do projeto (n�o a pasta bin)
var physicalPath = Path.Combine(builder.Environment.ContentRootPath, staticFilesPath);

// 3. Garante que o diret�rio exista no disco
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

app.UseHttpsRedirection(); // Recomendado adicionar para produ��o
app.UseStaticFiles(); // Para servir arquivos (como imagens de filmes)

app.UseRouting(); // Embora impl�cito, � bom saber que est� aqui

app.UseCors();

app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

app.Run();

/*
 * 
 * Diferen�as entre Manipulador JWT Tradicional (AddJwtBearer) e Novo Manipulador Identity (AddBearerToken)
 -A abordagem manual (.AddJwtBearer) sempre ser� necess�ria para cen�rios avan�ados onde a flexibilidade � mais 
importante que a simplicidade. / JWT (RFC 7519) / Total. Voc� controla cada par�metro de valida��o / Config: Manual e expl�cita (voc� define todas as regras)
/	Casos de Uso: APIs complexas, microservi�os, alta customiza��o / Sua responsabilidade total (armazenar, validar, revogar)

-A abordagem autom�tica (.AddBearerToken) ser� a escolha de 90% dos novos projetos, especialmente para desenvolvedores que 
est�o come�ando ou para aplica��es onde os padr�es de seguran�a da Microsoft s�o perfeitamente adequados (a grande maioria dos casos).
/ Tamb�m � JWT, mas gerenciado pelo Identity / Pode criar os seus ou usar os pr�-fabricados com .MapIdentityApi() / Limitado. A Microsoft abstrai os detalhes de voc�
/ Complexidade Muito Baixa. Ideal para desenvolvimento r�pido / Casos de uso: SPAs, aplicativos m�veis, projetos onde os padr�es s�o suficientes

 */
