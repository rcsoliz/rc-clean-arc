using Application.Behaviors;
using Application.DependencyInjection;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Interfaces;
using Application.Mapping;
using Application.Validators.Category;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using Presentation.Middleware;
using Prometheus;
using Serilog;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "SQL Server",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
     );

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Services swagger access by token in Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanArchitecture", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token en este formato: Bearer {token}"
    });

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

    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        return new[] { api.ActionDescriptor.RouteValues["controller"] };
    });

    c.DocInclusionPredicate((name, api) => true);
    
});


builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to Medditor.
builder.Services.AddMediatR(typeof(GetAllProductsQueryHandlers));


// Add Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add Service CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:7195") // 5173
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ── Rate Limiting ──────────────────────────────────────────
// Agregar en la sección de servicios:
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    //JSON para respuestas de rechazo personalizadas "Rate Limiting"
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        var response = new
        {
            status = 429,
            title = "Demasiadas solicitudes. Por favor espera un momento.",
            traceId = context.HttpContext.TraceIdentifier
        };

        var json = System.Text.Json.JsonSerializer.Serialize(response,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });

        await context.HttpContext.Response.WriteAsync(json, cancellationToken);
    };

    // Política para login — muy restrictiva (fuerza bruta)
    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Política para register — muy restrictiva
    options.AddFixedWindowLimiter("register", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 3;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Política para refresh-token
    options.AddFixedWindowLimiter("refresh", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Política general de lectura — por IP
    options.AddFixedWindowLimiter("read", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 60;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Política de escritura — por usuario autenticado
    options.AddFixedWindowLimiter("write", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 20;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// Register validadores de FluentValidation
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>();

builder.Services.AddControllers();

// Add conectivity to the database
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Infrastructure")));

// Add services to the container.
//builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7255"); // URL de tu API backend
});

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture v1");
        //c.RoutePrefix = string.Empty;
    });
}

// Mapea el endpoint /metrics
app.MapMetrics(); // Esto expone /metrics en formato Prometheus

// Mide todas las peticiones HTTP
app.UseHttpMetrics();

// Mapea el endpoint /metrics
app.MapMetrics(); // Esto expone /metrics en formato Prometheus

app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins); // Service CORS

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

//Health Checks
// Health Checks en formato Json para util para Dashboard Monitoreo
app.MapHealthChecks("/health", new HealthCheckOptions
{
     ResponseWriter = async (context, report) =>
     {
         context.Response.ContentType = "application/json";
         var result = JsonSerializer.Serialize(new
         {
             status = report.Status.ToString(),
             checks = report.Entries.Select(e => new {
                 name = e.Key,
                 status = e.Value.Status.ToString(),
                 duration = e.Value.Duration.ToString(),
                 description = e.Value.Description,
                 tags = e.Value.Tags.ToArray()
             }),
             totalDuration = report.TotalDuration.ToString()
         });
         await context.Response.WriteAsync(result);
     }
});

app.MapControllers();

app.Run();
