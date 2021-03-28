using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Modules;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests
{
    public class HelpersTests
    {
        /// <summary>
        /// Should register all services
        /// </summary>
        [Fact]
        public void ShouldRegisterAllModulesServices()
        {
            // 4 from A to D
            // 1 for the current list of modules
            const int numberOfServices = 5;
            
            var services = new ServiceCollection()
                .AddModule<A>()
                .AddModule<C>()
                ;

            Assert.Equal(numberOfServices, services.Count);
        }
    }
}