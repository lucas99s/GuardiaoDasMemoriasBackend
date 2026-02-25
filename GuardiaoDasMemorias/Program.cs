using GuardiaoDasMemorias.Data;
using GuardiaoDasMemorias.Models;
using GuardiaoDasMemorias.Repository.Commands.Cliente;
using GuardiaoDasMemorias.Repository.Commands.Contratos;
using GuardiaoDasMemorias.Repository.Commands.Memoria;
using GuardiaoDasMemorias.Repository.Commands.Musica;
using GuardiaoDasMemorias.Repository.Commands.Planos;
using GuardiaoDasMemorias.Repository.Commands.Tema;
using GuardiaoDasMemorias.Repository.Commands.Template;
using GuardiaoDasMemorias.Repository.Queries.Cliente;
using GuardiaoDasMemorias.Repository.Queries.Contratos;
using GuardiaoDasMemorias.Repository.Queries.Memoria;
using GuardiaoDasMemorias.Repository.Queries.Musica;
using GuardiaoDasMemorias.Repository.Queries.Planos;
using GuardiaoDasMemorias.Repository.Queries.Tema;
using GuardiaoDasMemorias.Repository.Queries.Template;
using GuardiaoDasMemorias.Services;
using GuardiaoDasMemorias.Services.CloudflareR2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Entity Framework com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Configurações de usuário
    options.User.RequireUniqueEmail = true;
    
    // Configurações de login
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configurar JWT Authentication
var jwtSecret = builder.Configuration["JwtSettings:Secret"] 
    ?? throw new InvalidOperationException("JWT Secret não configurado");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configurar Cloudflare R2
builder.Services.Configure<CloudflareR2Config>(
    builder.Configuration.GetSection("CloudflareR2"));
builder.Services.AddScoped<ICloudflareR2Service, CloudflareR2Service>();

// Registrar serviços
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>();

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
builder.Services.AddScoped<IPlanoCommands, PlanoCommands>();
builder.Services.AddScoped<IPlanoQueries, PlanoQueries>();
builder.Services.AddScoped<IContratoCommands, ContratoCommands>();
builder.Services.AddScoped<IContratoQueries, ContratoQueries>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Guardião das Memórias API",
        Version = "v1",
        Description = "API para gerenciamento de memórias"
    });

    // Configurar autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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

// IMPORTANTE: A ordem é crítica - UseCors ANTES de UseAuthentication/UseAuthorization
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
