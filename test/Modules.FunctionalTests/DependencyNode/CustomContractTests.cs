using System;
using Microsoft.Extensions.DependencyInjection;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.DependencyNode
{
    public class CustomContractTests
    {
        [Fact]
        public void Invoke_WithCustomContract_ShouldBeInvokedOnlyOnce()
        {
            var collection = new ServiceCollection();
            
            var tree = new DependencyTree(ModuleFactory.Default, typeof(CustomA));
            tree.ConfigureServices(collection);;

            var services = collection.BuildServiceProvider();
            var counter = services.GetRequiredService<Counter>();
            
            tree.Invoke<ICount>(counter);
            
            Assert.Equal(2, counter.Count);
        }
        
        private interface ICount
        {
            void Count(Counter counter);
        }

        [DependsOn(typeof(CustomB))]
        private class CustomA : IModule, ICount
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<Counter>();
            }

            public void Count(Counter counter)
            {
                counter.Count++;
            }
        }

        private class CustomB : IModule, ICount
        {
            public void ConfigureServices(IServiceCollection services)
            {
                // ..
            }

            public void Count(Counter counter)
            {
                counter.Count++;
            }
        }

        private class Counter
        {
            public int Count { get; set; }
        }
    }
}