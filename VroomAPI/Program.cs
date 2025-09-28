using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VroomAPI.Data;
using VroomAPI.Interface;
using VroomAPI.Mappings;
using VroomAPI.Service;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddAutoMapper(typeof(MotoMappingProfile));

builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IEventoService, EventoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "VroomAPI", 
        Version = "v1",
        Description = "Documentação VroomAPI",
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VroomAPI v1");
        c.DocumentTitle = "VroomAPI - Documentação";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
