namespace Hydrator.Api
{
    /// <summary>
    /// Defines a contract for objects whose selected members (methods or properties) can be populated by the hydrator service.
    /// Implement this interface to specify which members should be hydrated and what is required to perform the hydration.
    /// </summary>
    public interface IHydratableValue
    {
    }
}