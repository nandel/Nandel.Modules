using System;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Modules
{
    [DependsOn(typeof(C))]
    public class B : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ServiceB>();
        }
    }
}