using Microservicio.ReservasF.Api.Extensions;
using Microservicio.ReservasF.Api.Middleware;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// En local evitamos EventLog de Windows porque en este entorno rompe las requests
// por permisos de escritura sobre el log del sistema.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Controllers
builder.Services.AddControllers();

// Versioning
builder.Services.AddApiVersioningDocumentation();

// JWT Authentication: solo validación del token emitido por MS Seguridad
builder.Services.AddJwtAuthentication(builder.Configuration);

// CORS
builder.Services.AddCorsPolicy(builder.Configuration);

// Swagger
builder.Services.AddSwaggerDocumentation();

// Project services: DbContext + DataManagement + Business + Integrations
builder.Services.AddProjectServices(builder.Configuration);

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Swagger
app.UseSwaggerDocumentation();

// HTTPS
// En desarrollo evitamos redirección automática para no romper clientes
// que aún usan la URL http local (IIS Express).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCorsPolicy();

// Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();

// Global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Controllers
app.MapControllers();

app.Run();