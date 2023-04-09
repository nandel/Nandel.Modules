using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Nandel.Modules.FunctionalTests.DependencyInjection.ServiceProviderExtensions;

public class CustomContractTests
{
    [Fact]
    public void Invoke_WithCustomContract_ShouldBeInvokedOnlyOnce()
    {
        // arrange
        var services = new ServiceCollection()
            .AddRootModule<CustomA>()
            .BuildServiceProvider();
        
        var counter = services.GetRequiredService<Counter>();
        
        // act
        services.InvokeModulesContract<ICount>(counter);

        // assert
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