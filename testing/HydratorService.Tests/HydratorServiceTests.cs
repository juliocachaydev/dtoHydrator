using System.Reflection;
using Hydrator.Api;
using Hydrator.Tests.TestCommon;
using Microsoft.Extensions.DependencyInjection;

namespace HydratorService.Tests;

public class HydratorServiceTests : DiContainerTestBase
{
    [Fact]
    public async Task CanHydrate()
    {
        // ***** ARRANGE *****

        var serviceProvider = SetupServices(services =>
        {
            services.AddHydrator(o =>
            {
                o.EnsureAllDependenciesAreRegistered = true;
                o.AssertEachHydratableValueHasHandler = true;
            }, Assembly.GetExecutingAssembly());
        });

        using var scope = serviceProvider.CreateScope();

        var sut = scope.ServiceProvider.GetRequiredService<IHydratorService>();

        /*
         * I have an empty Dto, but there are two handlers, one sets the Dividend and Divisor properties, the other
         * calculates the Quotient. The first runs first because it is a high priority handler (which is important because otherwise we would
         * be dividing by zero).
         *
         * We expect the dto to be hydrated with the values set by the first handler and the division result set by the second handler.
         */
        var dto = new DtoExample();

        // ***** ACT *****

        await sut.HydrateAsync(dto);

        // ***** ASSERT *****

        Assert.True(dto.Dividend > 0);
        Assert.True(dto.Divisor > 0);

        Assert.Equal(dto.Dividend / dto.Divisor, dto.Quotient);
    }
}