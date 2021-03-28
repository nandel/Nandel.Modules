using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Modules;
using Modules.FunctionalTests.Services;
using Xunit;

namespace Modules.FunctionalTests.DependencyTree
{
    public class FunctionalTests
    {
        [Fact]
        public void RegisterServices_ShouldHave4RegistredServices()
        {
            var tree = new Nandel.Modules.DependencyTree(typeof(A));
            var services = new ServiceCollection();
            
            tree.RegisterServices(services);
            
            Assert.True(services.Count == 4, $"Collection had {services.Count} services");
        }

        [Fact]
        public void Initialize_ShouldOnlyBeInvokedOnceByModule()
        {
            var tree = new Nandel.Modules.DependencyTree(typeof(A));
            var collection = new ServiceCollection();
            
            tree.RegisterServices(collection);;

            var services = collection.BuildServiceProvider();
            tree.Initialize(services);

            var counter = services.GetRequiredService<ServiceC>();
            
            Assert.Equal(1, counter.Count);
        }

        [Fact]
        public void IsUsefull()
        {
            var a = new ServiceCollection();
            var b = new ServiceCollection();
            var module = new A();
            
            module.RegisterServices(a);

            for (var i = 0; i < 10; i++)
            {
                module.RegisterServices(b);
            }

            Assert.True(a.Count != b.Count, $"ServiceCollections have the same count");
        }
    }
}