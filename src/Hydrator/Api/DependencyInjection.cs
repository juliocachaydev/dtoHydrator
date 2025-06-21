using System;
using System.Reflection;
using Hydrator.Imp;
using Microsoft.Extensions.DependencyInjection;

namespace Hydrator.Api
{
    public static class DependencyInjection
    {
        public static void AddHydrator(this IServiceCollection services,
            params Assembly[] assembliesToScan)
        {
            services
                .AddSingleton<HandlerTypesRepository>(_ => new HandlerTypesRepository(assembliesToScan));

            services
                .AddScoped<IHydratorService, HydratorService>();
        }
    }
}