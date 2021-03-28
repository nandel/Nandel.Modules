using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Samples.Modules
{
    [DependsOn(typeof(D))]
    public class C : IModule, IHasInitialize, IHasStart, IHasStop
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ServiceC>();
        }


        public void Initialize(IServiceProvider services)
        {
            var service = services.GetRequiredService<ServiceC>();
            service.Count++;
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