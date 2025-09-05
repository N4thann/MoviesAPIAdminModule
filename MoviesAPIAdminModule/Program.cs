using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesAPIAdminModule;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;
using System.Text;

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

builder.Logging.ClearProviders(); //Remove todos os providers de logging configurados por padrão pelo ASP.NET Core (como EventLog, Console, Debug)
builder.Logging.AddConsole();//Adiciona o provider que exibe logs no console/terminal da aplicação 
builder.Logging.AddDebug();//Adiciona o provider que exibe logs na janela de Debug Output do Visual Studio

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Implementação para configuração utilizando JWT Bearer
var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddScoped<ApiLoggingFilter>();

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


app.UseStaticFiles();

//app.UseAuthorization(); Utilizando Bearer Token

app.MapControllers();

app.Run();
