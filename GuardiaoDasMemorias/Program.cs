using Microsoft.EntityFrameworkCore;
using GuardiaoDasMemorias.Data;
using GuardiaoDasMemorias.Repository.Queries.Cliente;
using GuardiaoDasMemorias.Repository.Commands.Cliente;
using GuardiaoDasMemorias.Repository.Queries.Memoria;
using GuardiaoDasMemorias.Repository.Commands.Memoria;
using GuardiaoDasMemorias.Repository.Queries.Tema;
using GuardiaoDasMemorias.Repository.Commands.Tema;
using GuardiaoDasMemorias.Repository.Queries.Musica;
using GuardiaoDasMemorias.Repository.Commands.Musica;
using GuardiaoDasMemorias.Repository.Queries.Template;
using GuardiaoDasMemorias.Repository.Commands.Template;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Entity Framework com PostgreSQL (apenas para migrations)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios Dapper
builder.Services.AddScoped<IClienteQueries, ClienteQueries>();
builder.Services.AddScoped<IClienteCommands, ClienteCommands>();
builder.Services.AddScoped<IMemoriaQueries, MemoriaQueries>();
builder.Services.AddScoped<IMemoriaCommands, MemoriaCommands>();
builder.Services.AddScoped<ITemaQueries, TemaQueries>();
builder.Services.AddScoped<ITemaCommands, TemaCommands>();
builder.Services.AddScoped<IMusicaQueries, MusicaQueries>();
builder.Services.AddScoped<IMusicaCommands, MusicaCommands>();
builder.Services.AddScoped<ITemplateQueries, TemplateQueries>();
builder.Services.AddScoped<ITemplateCommands, TemplateCommands>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Guardião das Memórias API",
        Version = "v1",
        Description = "API para gerenciamento de memórias"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guardião das Memórias API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz da aplicação
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
