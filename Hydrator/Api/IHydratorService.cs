using System.Threading.Tasks;

namespace Hydrator.Api
{
    /// <summary>
    /// Hydrates a DTO by setting the values using the handlers defined for the specified IHydratableObject or IHighPriorityHydratableObject.
    /// </summary>
    public interface IHydratorService
    {
        Task HydrateAsync<T>(T dto) where T : IHydratableObject;
    }
}