using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Hydrator.Api
{
    public static class DependencyInjection
    {
        public static void AddHydrator(this IServiceCollection services, Action<HydratorOptions> options,
            params Assembly[] assembliesToScan)
        {
        }
    }
}