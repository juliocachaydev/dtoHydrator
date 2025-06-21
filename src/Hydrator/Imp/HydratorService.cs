using System;
using System.Threading.Tasks;
using Hydrator.Api;

namespace Hydrator.Imp
{
    internal class HydratorService : IHydratorService

    {
        private readonly HandlerTypesRepository _handlerTypesRepository;
        private readonly IServiceProvider _sp;

        public HydratorService(
            HandlerTypesRepository handlerTypesRepository,
            IServiceProvider sp)
        {
            _handlerTypesRepository = handlerTypesRepository;
            _sp = sp;
        }
        
        public async Task HydrateAsync<T>(T dto) where T : IHydratableValue
        {
            var handlers = _handlerTypesRepository.GetStrategies(_sp, dto.GetType());
            
            foreach (var handler in handlers)
            {
                var hydrateMethod = handler.GetType().GetMethod("HydrateAsync")!;
                
                var result = hydrateMethod.Invoke(handler, new object[] { dto });
            
                if (result is Task task)
                {
                    await task;
                }
                
            }

        }
    }
}