using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Leer connection string — prioridad: variable de entorno DATABASE_URL > appsettings.json
            var connectionString = GetConnectionString(configuration);

            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            // Repositorios
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();

            // Servicios de autenticación y tokens
            services.AddScoped<IJwtService, JwtServiceRepository>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            // 1. Variable de entorno DATABASE_URL (Railway la inyecta automáticamente)
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (!string.IsNullOrEmpty(databaseUrl))
            {
                return ConvertPostgresUrlToNpgsql(databaseUrl);
            }

            // 2. Variable de entorno directa en formato Npgsql
            var envConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            if (!string.IsNullOrEmpty(envConnection))
            {
                return envConnection;
            }

            // 3. appsettings.json (desarrollo local)
            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No connection string found.");
        }

        private static string ConvertPostgresUrlToNpgsql(string url)
        {
            // Convierte: postgresql://user:password@host:port/database
            // A: Host=host;Port=port;Database=database;Username=user;Password=password
            try
            {
                var uri = new Uri(url);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                var host = uri.Host;
                var port = uri.Port > 0 ? uri.Port : 5432;
                var database = uri.AbsolutePath.TrimStart('/');

                return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
            }
            catch
            {
                return url;
            }
        }
    }
}