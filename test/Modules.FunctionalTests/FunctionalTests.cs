using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Modules;
using Xunit;

namespace Modules.FunctionalTests
{
    public class FunctionalTests
    {
        [Fact]
        public void IsUsefull()
        {
            var a = new ServiceCollection();
            var b = new ServiceCollection();
            var module = new A();
            
            module.RegisterServices(a);

            for (var i = 0; i < 10; i++)
            {
                module.RegisterServices(b);
            }

            Assert.True(a.Count != b.Count, $"ServiceCollections have the same count");
        }
    }
}