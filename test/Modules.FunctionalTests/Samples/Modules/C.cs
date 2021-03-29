using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Samples.Modules
{
    [DependsOn(typeof(D))]
    public class C : IModule, IHasStart, IHasStop
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ServiceC>();
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
}