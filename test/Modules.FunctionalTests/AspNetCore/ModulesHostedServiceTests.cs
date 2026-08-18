using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nandel.Modules.AspNetCore;
using Xunit;

namespace Nandel.Modules.FunctionalTests.AspNetCore;

public class ModulesHostedServiceTests
{
    [Fact]
    public async Task StartAsync_ShouldStartModulesImplementingIHasStart()
    {
        // arrange
        var provider = BuildServiceProvider();
        var hostedService = provider.GetRequiredService<IHostedService>();

        // act
        await hostedService.StartAsync(CancellationToken.None);

        // assert
        Assert.Equal(1, provider.GetRequiredService<Tracker>().Started);
    }

    [Fact]
    public async Task StopAsync_ShouldStopModulesImplementingIHasStop()
    {
        // arrange
        var provider = BuildServiceProvider();
        var hostedService = provider.GetRequiredService<IHostedService>();

        // act
        await hostedService.StopAsync(CancellationToken.None);

        // assert
        Assert.Equal(1, provider.GetRequiredService<Tracker>().Stopped);
    }

    private static ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddRootModule<StartableModule>()
            .AddModulesHostedService()
            .BuildServiceProvider();
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
}
