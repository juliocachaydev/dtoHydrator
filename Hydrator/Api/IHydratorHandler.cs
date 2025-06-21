using System.Threading.Tasks;

namespace Hydrator.Api
{
    /// <summary>
    /// Provides an implementation that encapsulates the logic for hydrating a specific type of IHydratableObject.
    /// You must provide up to one HydratorHandler for each IHydratableObject type.
    /// </summary>
    public interface IHydratorHandler<T> where T : IHydratableObject
    {
        Task HydrateAsync(T dto);
    }
}