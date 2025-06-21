using Hydrator.Api;

namespace Hydrator.Tests.ProjectWithMissingRegisteredDependency;

public static class CustomerNameWhoseHandlerIsMissingADependency
{
/*
 * In this project, there is a hydratableValue with handler that has one dependency, which the test will register or not to ensure
 * the library can asserts that there are no missing dependencies.
 */
    public interface ICustomerName : IHydratableValue
    {
    }

    public class Handler : IHydratorHandler<ICustomerName>
    {
        public Handler(SomeDependency someDependency)
        {
        }

        public Task HydrateAsync(ICustomerName dto)
        {
            throw new NotImplementedException();
        }
    }
}