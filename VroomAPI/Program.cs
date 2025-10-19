using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VroomAPI.Authentication;
using VroomAPI.Data;
using VroomAPI.Interface;
using VroomAPI.Mappings;
using VroomAPI.Service;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: connectionString,
        name: "oracle-database",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "db", "oracle", "sql" },
        timeout: TimeSpan.FromSeconds(10)
    ).AddUrlGroup(
        uri: new Uri("http://127.0.0.1:1880"),
        name: "node-red api",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "api", "node-red", "external" },
        timeout: TimeSpan.FromSeconds(5)
    );
builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(5);
    opt.MaximumHistoryEntriesPerEndpoint(10);
    opt.AddHealthCheckEndpoint("API Health Check", "/health");
}).AddInMemoryStorage();

builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1.0", new OpenApiInfo 
    { 
        Title = "VroomAPI", 
        Version = "v1.0",
        Description = "Documentação VroomAPI v1.0 (Deprecated)",
    });
    
    c.SwaggerDoc("v2.0", new OpenApiInfo 
    { 
        Title = "VroomAPI", 
        Version = "v2.0",
        Description = "Documentação VroomAPI v2.0",
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2.0);
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("api-version")
    );
    options.ReportApiVersions = true;
    options.ApiVersionSelector = new DefaultApiVersionSelector(options);

}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-dashboard";
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "VroomAPI v2.0");
        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "VroomAPI v1.0 (Deprecated)");
        c.DocumentTitle = "VroomAPI - Documentação";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
