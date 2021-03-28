using System;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Modules
{
    [DependsOn(typeof(D))]
    public class C : IModule, IHasInitialize
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
    }
}