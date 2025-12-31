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
using GuardiaoDasMemorias.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Entity Framework com PostgreSQL (apenas para migrations)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar serviços
builder.Services.AddScoped<IHashService, HashService>();

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

// Configure CORS - Permite requisições do frontend em desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            var allowedOrigins = new List<string>
            {
                "http://localhost:5173",
                "http://localhost:3000",
                "http://localhost:4200",
                "http://localhost:5174",
                "https://guardiaodasmemorias.up.railway.app",
                "https://guardiaodasmemoriasbackend-production.up.railway.app"
            };

            // Adicionar origem do ambiente se configurada
            var frontendUrl = builder.Configuration["FRONTEND_URL"];
            if (!string.IsNullOrEmpty(frontendUrl))
            {
                allowedOrigins.Add(frontendUrl);
            }

            policy.WithOrigins(allowedOrigins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
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

// IMPORTANTE: UseCors DEVE vir ANTES de UseHttpsRedirection e UseAuthorization
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
