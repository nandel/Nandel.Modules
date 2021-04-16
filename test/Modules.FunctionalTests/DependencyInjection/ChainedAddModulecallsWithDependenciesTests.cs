using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.DependencyInjection
{
    public class ChainedAddModulecallsWithDependenciesTests
    {
        [Fact]
        public void AddModules_WithChainedCalls_ShouldConfigureAllServices()
        {
            // arrange
            const int serviceCount = 3; // 2 from the actual services and 1 for the tree
            var services = new ServiceCollection();
            var configuration = new Mock<IConfiguration>().Object;

            // act
            services.AddModule<A>(new[]
            {
                configuration
            });

            // assert
            Assert.Equal(serviceCount, services.Count);
        }

        private class A : IModule
        {
            public A(IConfiguration configuration) {}
            
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddModule<B>();
                services.AddSingleton<Service>();
            }
            
            private class Service {}
        }

        private class B : IModule
        {
            public B(IConfiguration configuration) {}
            
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<Service>();
            }
            
            private class Service {}
        }
    }
}