namespace Hydrator.Api
{
    /// <summary>
    /// Defines a contract for objects that require prioritized hydration by the hydrator service.
    /// Implement this interface when certain values must be hydrated first, allowing their use in subsequent hydration steps of other objects.
    /// </summary>
    public interface IHighPriorityHydratableValue : IHydratableValue
    {
        
    }
}