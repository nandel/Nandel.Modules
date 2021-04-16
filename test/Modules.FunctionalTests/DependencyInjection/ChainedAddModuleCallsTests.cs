using Microsoft.Extensions.DependencyInjection;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.DependencyInjection
{
    public class ChainedAddModuleCallsTests
    {
        [Fact]
        public void AddModules_WithChainedCalls_ShouldConfigureAllServices()
        {
            // arrange
            const int serviceCount = 3; // 2 from the actual services and 1 for the tree
            var services = new ServiceCollection();

            // act
            services.AddModule<A>();

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
            
            private class Service {}
        }

        private class B : IModule
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<Service>();
            }
            
            private class Service {}
        }
    }
}