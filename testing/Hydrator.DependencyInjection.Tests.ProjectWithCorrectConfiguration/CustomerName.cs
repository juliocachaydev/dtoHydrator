using Hydrator.Api;

namespace Hydrator.Tests.ProjectWithCorrectConfiguration;

public static class CustomerName
{
/*
 * In this project, there is a hydratableValue but there is no handler for it, this is intended to test
 * that the library will assert that each HydratableValue has a corresponding handler.
 */
    public interface ICustomerName : IHydratableValue
    {
    }

    public class Handler : IHydratorHandler<ICustomerName>
    {
        public Task HydrateAsync(ICustomerName dto)
        {
            throw new NotImplementedException();
        }
    }
}