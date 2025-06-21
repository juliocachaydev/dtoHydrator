

using Microsoft.Extensions.DependencyInjection;

namespace Hydrator.Tests.TestCommon;

public abstract class DiContainerTestBase
{
    
    protected IServiceProvider SetupServices(
        Action<IServiceCollection> configurationAction)
    {
        var serviceCollection = new ServiceCollection();
        
        configurationAction(serviceCollection);
        
        var serviceProvider = serviceCollection
            .BuildServiceProvider();

        return serviceProvider;
    }

}