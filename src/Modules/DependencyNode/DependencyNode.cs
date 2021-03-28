using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public class DependencyNode : IDependencyNode
    {
        public Type ModuleType { get; }
        public IDependencyNode Root { get; }
        public DependencyList Dependencies { get; }

        private readonly ModuleFactory _factory;

        private bool _registredServices;
        private bool _initialized;
        private bool _started;
        private bool _stoped;
        
        private object _moduleInstance;

        public DependencyNode(ModuleFactory factory, Type moduleType, IDependencyNode root)
        {
            _factory = factory;
            
            ModuleType = moduleType;
            Root = root;
            
            Dependencies = new DependencyList(factory, Root);
        }

        public void AddDependencies()
        {
            var dependencies = ModuleType.GetCustomAttributes(inherit: true)
                .OfType<DependsOnAttribute>()
                .SelectMany(x => x.ModuleTypes)
                .ToList()
                ;

            Dependencies.AddRange(dependencies);
        }

        private T GetModuleInstance<T>() where T : class
        {
            if (_moduleInstance is null)
            {
                _moduleInstance = _factory.CreateInstance(ModuleType);
            }

            return _moduleInstance as T;
        }

        
        public IDependencyNode FindNode(Type moduleType)
        {
            if (ModuleType == moduleType)
            {
                return this;
            }

            return Dependencies.FindNode(moduleType);
        }
        
        public void RegisterServices(IServiceCollection services)
        {
            if (_registredServices) return;
            
            _registredServices = true;
            Dependencies.RegisterServices(services);

            GetModuleInstance<IModule>().RegisterServices(services);
        }

        public void Initialize(IServiceProvider services)
        {
            if (_initialized) return;

            _initialized = true;
            Dependencies.Initialize(services);
            
            GetModuleInstance<IHasInitialize>()?.Initialize(services);
        }

        public async Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            if (_stoped) return;
            
            cancellationToken.ThrowIfCancellationRequested();

            _stoped = true;
            _started = false;

            await (GetModuleInstance<IHasStop>()?.StopAsync(services, cancellationToken) ?? Task.CompletedTask);
            await Dependencies.StopAsync(services, cancellationToken);
        }

        public async Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            if (_started) return;
            
            cancellationToken.ThrowIfCancellationRequested();

            _started = true;
            _stoped = false;

            await Dependencies.StartAsync(services, cancellationToken);
            await (GetModuleInstance<IHasStart>()?.StartAsync(services, cancellationToken) ?? Task.CompletedTask);
        }
    }
}