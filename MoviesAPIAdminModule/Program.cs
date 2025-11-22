using Asp.Versioning;
using Microsoft.Extensions.FileProviders;
using MoviesAPIAdminModule.Extensions;
using MoviesAPIAdminModule.Filters;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor(); // Needed for UseStaticFiles
//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions()); // Needed for AWS S3
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.PostProcess = document =>
    {
        document.Info.Title = "Movies API Admin Module";
        document.Info.Version = "v1";
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

    // Define the security scheme for JWT Bearer tokens in Intecegration with Swagger / NSwag
    settings.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Insira o token JWT: Bearer {seu_token}",
    });

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

builder.Logging.ClearProviders(); // Remove all logging providers configured by default by ASP.NET Core (such as EventLog, Console, Debug)
builder.Logging.AddConsole(); // Adds the provider that writes logs to the application's console/terminal
builder.Logging.AddDebug(); // Adds the provider that writes logs to the Visual Studio Debug Output window

// Calls to extension methods
// 1. Register infrastructure services first (including AddIdentity)
// 2. Register application services
// 3. Register Web API services last so that JWT configuration overrides Identity defaults.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureExceptionHandler();
    // 1. The Generator (where the .json file is created)
    // We need to tell it to use the SAME path that the UI expects.
    app.UseOpenApi(settings =>
    {
        settings.Path = "/openapi/{documentName}/openapi.json";
    });

    // 2. The UI (where the user sees it)
    // (Yours was almost correct, pointing to the correct path)
    app.UseSwaggerUi(settings =>
    {
        // This path MUST be the same as the 'settings.Path' above
        settings.DocumentPath = "/openapi/{documentName}/openapi.json";
        settings.DocumentTitle = "Movies API - Docs";
    });
}

#region ===== FIXED STATIC FILES CONFIGURATION =====

// 1. Get the relative path from appsettings.json
var staticFilesPath = builder.Configuration.GetValue<string>("FileStorageSettings:LocalUploadPath");

if (string.IsNullOrEmpty(staticFilesPath))
    throw new InvalidOperationException("The key 'FileStorageSettings:LocalUploadPath' is not configured in appsettings.json.");


// 2. Build the full physical path using the project's root (not the bin folder)
var physicalPath = Path.Combine(builder.Environment.ContentRootPath, staticFilesPath);

// 3. Ensure the directory exists on disk
if (!Directory.Exists(physicalPath))
    Directory.CreateDirectory(physicalPath);


// 4. Configure the middleware to serve files from the correct folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(physicalPath),
    RequestPath = $"/{staticFilesPath.Replace("\\", "/")}"
});

#endregion

app.UseHttpsRedirection(); // Recommended to enable for production
app.UseStaticFiles(); // To serve files (such as movie images)

app.UseRouting(); // Although implicit, it's good to know it's here
app.UseRateLimiter();

app.UseCors("AllowMyClient");

app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

app.Run();

