using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nandel.Modules.FunctionalTests.Samples.Services;

namespace Nandel.Modules.FunctionalTests.Samples.Modules;

[DependsOn(typeof(D))]
public class C : IModule, IHasStart, IHasStop
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.TryAddSingleton<ServiceC>();
    }

    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var service = services.GetRequiredService<ServiceC>();
        service.Count++;

        return Task.CompletedTask;
    }

    public Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var service = services.GetRequiredService<ServiceC>();
        service.Count++;

        return Task.CompletedTask;
    }
}