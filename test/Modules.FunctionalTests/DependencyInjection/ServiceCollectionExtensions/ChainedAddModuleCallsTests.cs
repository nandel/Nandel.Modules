using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Nandel.Modules.FunctionalTests.DependencyInjection.ServiceCollectionExtensions;

public class ChainedAddModuleCallsTests
{
    [Fact]
    public void AddRootModule_WithChainedCalls_ShouldConfigureAllServices()
    {
        // arrange
        const int serviceCount = 3; // 2 from the actual services and 1 for root DependencyNode
        var services = new ServiceCollection();

        // act
        services.AddRootModule<A>();

        // assert
        Assert.Equal(serviceCount, services.Count);
    }

    private class A : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModule<B>();
            services.AddSingleton<Service>();
        }

        private class Service
        {
        }
    }

    private class B : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Service>();
        }

        private class Service
        {
        }
    }
}