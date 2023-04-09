using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules;

public static class ServiceProviderExtensions
{
    public static async Task StartModulesAsync(this IServiceProvider services, CancellationToken cancellationToken)
    {
        var controllers = GetDependencyControllers(services);
        foreach (var controller in controllers)
        {
            await controller.StartAsync(services, cancellationToken);
        }
    }
    
    public static async Task StopModulesAsync(this IServiceProvider services, CancellationToken cancellationToken)
    {
        var controllers = GetDependencyControllers(services);
        foreach (var controller in controllers)
        {
            await controller.StopAsync(services, cancellationToken);
        }
    }

    public static void InvokeModulesContract<T>(this IServiceProvider services, params object[] args) where T: class
    {
        var controllers = GetDependencyControllers(services);
        foreach (var controller in controllers)
        {
            controller.Invoke<T>(args);
        }
    }

    private static IEnumerable<DependencyController> GetDependencyControllers(IServiceProvider services)
    {
        var dependencyNode = services.GetRequiredService<DependencyNode>();
        var controllers = dependencyNode.AsEnumerable()
            .Select(x => x.Controller);

        return controllers;
    }
}