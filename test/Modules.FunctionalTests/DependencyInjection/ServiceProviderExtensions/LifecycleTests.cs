using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Nandel.Modules.FunctionalTests.DependencyInjection.ServiceProviderExtensions;

public class LifecycleTests
{
    [Fact]
    public async Task StartModulesAsync_ShouldStartOnlyModulesImplementingIHasStart()
    {
        // arrange
        var services = new ServiceCollection()
            .AddRootModule<StartableModule>()
            .AddModule<PlainModule>()
            .BuildServiceProvider();

        var tracker = services.GetRequiredService<Tracker>();

        // act
        await services.StartModulesAsync(CancellationToken.None);

        // assert
        Assert.Equal(1, tracker.Started);
    }

    [Fact]
    public async Task StopModulesAsync_ShouldStopOnlyModulesImplementingIHasStop()
    {
        // arrange
        var services = new ServiceCollection()
            .AddRootModule<StartableModule>()
            .AddModule<PlainModule>()
            .BuildServiceProvider();

        var tracker = services.GetRequiredService<Tracker>();

        // act
        await services.StopModulesAsync(CancellationToken.None);

        // assert
        Assert.Equal(1, tracker.Stopped);
    }

    private class Tracker
    {
        public int Started { get; set; }
        public int Stopped { get; set; }
    }

    private class StartableModule : IModule, IHasStart, IHasStop
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Tracker>();
        }

        public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            services.GetRequiredService<Tracker>().Started++;
            return Task.CompletedTask;
        }

        public Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            services.GetRequiredService<Tracker>().Stopped++;
            return Task.CompletedTask;
        }
    }

    private class PlainModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
