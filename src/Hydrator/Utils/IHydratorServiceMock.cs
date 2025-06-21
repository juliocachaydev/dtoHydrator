using System.Collections.Generic;
using System.Threading.Tasks;
using Hydrator.Api;

namespace Hydrator.Utils
{
    /// <summary>
    /// Utility class to mock the IHydratorService for testing purposes.
    /// </summary>
    public class IHydratorServiceMock
    {
        private TestHydrator _mockObject = new TestHydrator();

        public IHydratorService Object => _mockObject;
        
        public bool WasCalled<T>(T dto) where T : IHydratableValue
        {
            return _mockObject.Dtos.Contains(dto);
        }
   
    }

    class TestHydrator : IHydratorService
    {
        public List<object> Dtos { get; } = new List<object>();
        public Task HydrateAsync<T>(T dto) where T : IHydratableValue
        {
            Dtos.Add(dto);
            return Task.CompletedTask;
        }
    }
}