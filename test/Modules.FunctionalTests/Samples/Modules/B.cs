using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Services;
using Nandel.Modules;

namespace Modules.FunctionalTests.Samples.Modules
{
    [DependsOn(typeof(C))]
    public class B : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ServiceB>();
        }
    }
}