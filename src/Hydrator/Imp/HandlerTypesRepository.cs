using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hydrator.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Hydrator.Imp
{
    internal class HandlerTypesRepository
    {
        // Key is the dto type, value is the strategy type
        private readonly Dictionary<Type, Type> _strategies = new Dictionary<Type, Type>();


        public HandlerTypesRepository(
            params Assembly[] assembliesToScan)
        {
            _strategies = assembliesToScan
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract) // Ensure it's a concrete class
                .SelectMany(x => x.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHydratorHandler<>))
                    .Select(i => new { StrategyType = x, HydratableValueType = i.GetGenericArguments()[0] }))
                .ToDictionary(x => x.HydratableValueType, x => x.StrategyType);

        }

        public object[] GetStrategies(IServiceProvider sp, Type dtoType)
        {
            var hydratables = dtoType.GetInterfaces()
                .Where(x => typeof(IHydratableValue).IsAssignableFrom(x))
                .ToArray();
        
            var result = new List<object>();

            foreach (var hydratable in hydratables)
            {
                if (_strategies.TryGetValue(hydratable, out var strategyType))
                {
                    result.Add(ActivatorUtilities.CreateInstance(sp, strategyType!));
                }
            }

            return result.ToArray();
        }
    }
}