using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Nandel.Modules.FunctionalTests.DependencyInjection.ServiceCollectionExtensions;

public class ChainedAddModuleCallsWithDependenciesTests
{
    [Fact]
    public void AddRootModule_WithChainedCalls_ShouldConfigureAllServices()
    {
        // arrange
        const int serviceCount = 3; // 2 from the actual services and 1 for root DependencyNode
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>().Object;

        // act
        services.AddRootModule<A>(new[]
        {
            configuration
        });

        // assert
        Assert.Equal(serviceCount, services.Count);
    }

    private class A : IModule
    {
        public A(IConfiguration configuration)
        {
        }

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
        public B(IConfiguration configuration)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Service>();
        }

        private class Service
        {
        }
    }
}