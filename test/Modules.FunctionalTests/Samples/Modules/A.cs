using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nandel.Modules.FunctionalTests.Samples.Services;

namespace Nandel.Modules.FunctionalTests.Samples.Modules;

[DependsOn(
    typeof(C),
    typeof(B)
)]
public class A : IModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.TryAddTransient<ServiceA>();
    }
}