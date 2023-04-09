using Microsoft.Extensions.DependencyInjection;
using Nandel.Modules.FunctionalTests.Samples.Modules;
using Xunit;

namespace Nandel.Modules.FunctionalTests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Should register all services
    /// </summary>
    [Fact]
    public void AddModule_WithExplicitlyDuplicatedModule_ShouldRegisterAllModulesServices()
    {
        // 4 from A to D
        // 1 for the current list of modules
        const int numberOfServices = 5;

        var services = new ServiceCollection()
            .AddRootModule<A>(ModuleFactory.Default)
            .AddModule<C>()
            ;

        Assert.Equal(numberOfServices, services.Count);
    }
}