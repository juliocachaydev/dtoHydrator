using System.Reflection;
using Hydrator.Api;
using Hydrator.Tests.ProjectWithCorrectConfiguration;
using Hydrator.Tests.ProjectWithMissingRegisteredDependency;
using Hydrator.Tests.ProjectWithOneMissingHandler;
using Hydrator.Tests.TestCommon;
using Microsoft.Extensions.DependencyInjection;

namespace Hydrator.Tests.Api;

public class DependencyInjectionTests : DiContainerTestBase
{
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public void Asserts_AllHydratableValues_HaveCorrespondingHandlers(
        bool configurationRequiresAssertion, bool oneHandlerIsMissingInSpecifiedAssembly, bool shouldThrowException)
    {
        // ***** ARRANGE *****

        // In this test, we will check if the library asserts that each HydratableValue has a handler. We do so by adding the library using a different
        // assembly for each test case.
        var assemblyToScan = oneHandlerIsMissingInSpecifiedAssembly
            ? typeof(CustomerNameWithoutHandlerHydrator).Assembly
            : Assembly.GetAssembly(
                typeof(CustomerName));

        // ***** ACT *****

        var result = Record.Exception(() =>
        {
            SetupServices(services =>
            {
                services.AddHydrator(options =>
                    {
                        // The client can specify whether to assert that each HydratableValue has a handler.
                        options.AssertEachHydratableValueHasHandler = configurationRequiresAssertion;
                    },
                    assemblyToScan!);
            });
        });

        // ***** ASSERT *****

        if (shouldThrowException)
        {
            Assert.NotNull(result);
            Assert.IsType<HydratorException>(result);
            Assert.Matches("Handler not found", result.Message);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public void Asserts_AllHandlerDependencies_AreRegisteredInServiceProvider(
        bool configurationRequiresAssertion, bool oneHandlerIsMissingADependency, bool shouldThrowException)
    {
        // ***** ARRANGE *****

        // ***** ACT *****

        var result = Record.Exception(
            () =>
            {
                SetupServices(services =>
                {
                    if (!oneHandlerIsMissingADependency) services.AddScoped<SomeDependency>();

                    services.AddHydrator(
                        options => { options.EnsureAllDependenciesAreRegistered = configurationRequiresAssertion; },
                        Assembly.GetAssembly(typeof(CustomerNameWhoseHandlerIsMissingADependency))!);
                });
            });

        // ***** ASSERT *****

        if (shouldThrowException)
        {
            Assert.NotNull(result);
            Assert.IsType<HydratorException>(result);
            Assert.Matches("Missing", result.Message);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Fact]
    public void CanCreate_ScopedHydratorService()
    {
        /*
         * We are going to test that the services is scoped. To do this, we will get two instances from the same services scope and verify
         * they are the same instance, then, we will get two instances from different service scopes and verify they are different.
         */

        // ***** ARRANGE *****

        var serviceProvider = SetupServices(services =>
        {
            services.AddHydrator(options => { },
                Assembly.GetAssembly(typeof(CustomerName))!);
        });

        // ***** ACT *****

        using var scope1 = serviceProvider.CreateScope();

        var instance1 = scope1.ServiceProvider.GetRequiredService<IHydratorService>();
        var instance2 = scope1.ServiceProvider.GetRequiredService<IHydratorService>();

        using var scope2 = serviceProvider.CreateScope();
        var instanceFromDifferentScope = scope2.ServiceProvider.GetRequiredService<IHydratorService>();

        // ***** ASSERT *****

        // Same scope, same instance (Not a transient)
        Assert.Same(instance1, instance2);

        // Different scope, different instance (not a singleton)
        Assert.NotSame(instance1, instanceFromDifferentScope);

        // Because is not transient, and because is not singleton, then it must be scoped.
    }
}