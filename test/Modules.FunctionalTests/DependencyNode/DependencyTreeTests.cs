using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modules.FunctionalTests.Samples.Modules;
using Modules.FunctionalTests.Samples.Services;
using Moq;
using Nandel.Modules;
using Xunit;

namespace Modules.FunctionalTests.DependencyNode
{
    public class DependencyTreeTests
    {
        [Fact]
        public void GetNodes_ShouldReturn4Nodes()
        {
            // we should have 4 nodes since we register
            // A to D, even thought we register C twice
            
            // Arrange
            var tree = new DependencyTree(ModuleFactory.Default, typeof(A));
            
            // Act
            var subject = tree.GetNodes();
            
            // Assert
            Assert.Equal(4, subject.Count());
        }
        
        [Fact]
        public void GetNodes_ShouldReturnInTheExpectedOrder()
        {
            // we should have 4 nodes since we register
            // A to D, even thought we register C twice
            
            // Arrange
            var tree = new DependencyTree(ModuleFactory.Default, typeof(A));
            var expected = new List<Type>()
            {
                typeof(D),
                typeof(C),
                typeof(B),
                typeof(A)
            };
            
            // Act
            var subject = tree.GetNodes()
                .Select(x => (x as Nandel.Modules.DependencyNode)?.ModuleType)
                .Where(x => x is not null)
                .ToList()
                ;
            
            // Assert
            Assert.True(subject.SequenceEqual(expected), "Not returned in the expected order");
        }
        
        [Fact]
        public void ConfigureServices_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // Since we register from A to D we should invoke 4 times

            // Arrange
            var tree = new DependencyTree(ModuleFactory.Default, typeof(A));
            
            var services = new Mock<IServiceCollection>();
            services.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Verifiable()
                ;

            // Act
            tree.ConfigureServices(services.Object);
            
            // Assert
            services.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), () => Times.Exactly(4));
        }

        [Fact]
        public async Task StartAsync_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // and has a ServiceC singleton with a counter
            // so we can check how many times it was invoked
            
            // Arrange
            var tree = new DependencyTree(ModuleFactory.Default, typeof(A));
            var collection = new ServiceCollection();
            tree.ConfigureServices(collection);
            var services = collection.BuildServiceProvider();
            
            // Act
            await tree.StartAsync(services, CancellationToken.None);

            // Assert
            Assert.Equal(1, services.GetRequiredService<ServiceC>().Count);
        }
        
        [Fact]
        public async Task StopAsync_ShouldBeInvokedOnlyOnceByModule()
        {
            // Module `C` is registred twice in the tree
            // and has a ServiceC singleton with a counter
            // so we can check how many times it was invoked
            
            // Arrange
            var tree = new DependencyTree(ModuleFactory.Default, typeof(A));
            var collection = new ServiceCollection();
            tree.ConfigureServices(collection);
            var services = collection.BuildServiceProvider();
            
            // Act
            await tree.StopAsync(services, CancellationToken.None);

            // Assert
            Assert.Equal(1, services.GetRequiredService<ServiceC>().Count);
        }
    }
}