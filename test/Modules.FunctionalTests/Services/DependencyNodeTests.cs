using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nandel.Modules.FunctionalTests.Samples.Modules;
using Xunit;

namespace Nandel.Modules.FunctionalTests.Services;

public class DependencyNodeTests
{
    [Fact]
    public void AsEnumerable_ShouldReturn4Nodes()
    {
        // we should have 4 nodes since we register
        // A to D, even thought we register C twice

        // Arrange
        var node = new Modules.DependencyNode(typeof(A), ModuleFactory.Default);

        // Act
        var subject = node.AsEnumerable();

        // Assert
        Assert.Equal(4, subject.Count());
    }

    [Fact]
    public void AsEnumerable_ShouldReturnInTheExpectedOrder()
    {
        // we should have 4 nodes since we register
        // A to D, even thought we register C twice

        // Arrange
        var node = new Modules.DependencyNode(typeof(A), ModuleFactory.Default);
        var expected = new List<Type>()
        {
            typeof(D),
            typeof(C),
            typeof(B),
            typeof(A)
        };

        // Act
        var subject = node.AsEnumerable()
            .Select(x => x.ModuleType)
            .ToList();

        // Assert
        Assert.True(subject.SequenceEqual(expected), "Not returned in the expected order");
    }

    [Fact]
    public void ConfigureServices_ShouldBeInvokedOnlyOnceByModule()
    {
        // Module `C` is registered twice in the tree
        // Since we register from A to D we should invoke 4 times

        // Arrange
        var node = new Modules.DependencyNode(typeof(A), ModuleFactory.Default);

        var services = new Mock<IServiceCollection>();
        
        services.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
            .Verifiable();

        // Act
        node.ConfigureServices(services.Object);

        // Assert
        services.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), () => Times.Exactly(4));
    }
}