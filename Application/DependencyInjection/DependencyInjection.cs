using Microsoft.Extensions.DependencyInjection;
using MediatR;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // MediatR
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Otros servicios de aplicación
            //services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
