using System;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Modules
{
    [DependsOn(typeof(B), typeof(C))]
    public class A : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ServiceA>();
        }
    }
}