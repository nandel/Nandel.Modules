using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nandel.Modules;

public sealed class DependencyController(Type moduleType, ModuleFactory factory)
{
    private readonly object _moduleInstance = factory.CreateInstance(moduleType) ?? throw new InvalidOperationException($"Can't instantiate {moduleType}");
    private bool _configureServicesInvoked;

    public void ConfigureServices<T>(T services)
    {
        if (_configureServicesInvoked) return;
        if (_moduleInstance is not IModule<T> strongTypedModule) throw new InvalidOperationException($"{_moduleInstance.GetType()} doesn't implement IModule<{typeof(T).Name}>");

        _configureServicesInvoked = true;
        strongTypedModule.ConfigureServices(services);
    }

    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        if (_moduleInstance is IHasStart startModule)
        {
            return startModule.StartAsync(services, cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        if (_moduleInstance is IHasStop stopModule)
        {
            return stopModule.StopAsync(services, cancellationToken);
        }

        return Task.CompletedTask;
    }
    
    public void Invoke<T>(params object[] args) where T : class
    {
        if (!typeof(T).IsAssignableFrom(moduleType)) return;

        var methods = typeof(T).GetMethods();
        if (methods.Length != 1)
        {
            throw new InvalidOperationException("Module contracts can only define one method");
        }

        methods.First().Invoke(_moduleInstance, args);
    }
}