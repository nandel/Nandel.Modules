using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Samples.Modules
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