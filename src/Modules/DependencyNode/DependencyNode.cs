using System;
using System.Collections;
using System.Collections.Generic;
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
        public DependencyTree Dependencies { get; }

        private readonly ModuleFactory _factory;
        private readonly IDictionary<Type, bool> _invoked = new Dictionary<Type, bool>();
        
        private object _moduleInstance;

        private bool _registred;
        private bool _started;
        private bool _stoped;
        
        public DependencyNode(ModuleFactory factory, Type moduleType, IDependencyNode root)
        {
            _factory = factory;
            
            ModuleType = moduleType;
            Root = root;
            
            Dependencies = new DependencyTree(factory, Root);
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

        public IEnumerable<IDependencyNode> GetNodes()
        {
            return new List<IDependencyNode>(Dependencies.GetNodes()) 
                { this }
                .Distinct()
                ;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            if (_registred) return;

            _registred = true;
            
            Dependencies.ConfigureServices(services);
            GetModuleInstance<IModule>().ConfigureServices(services);
        }

        public async Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            if (_stoped) return;

            _started = false;
            _stoped = true;
            
            cancellationToken.ThrowIfCancellationRequested();

            await (GetModuleInstance<IHasStop>()?.StopAsync(services, cancellationToken) ?? Task.CompletedTask);
            await Dependencies.StopAsync(services, cancellationToken);
        }

        public async Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            if(_started) return;

            _started = true;
            _stoped = false;
            
            cancellationToken.ThrowIfCancellationRequested();

            await Dependencies.StartAsync(services, cancellationToken);
            await (GetModuleInstance<IHasStart>()?.StartAsync(services, cancellationToken) ?? Task.CompletedTask);
        }

        public void Invoke<T>(params object[] args) where T : class
        {
            if (!typeof(T).IsAssignableFrom(ModuleType)) return;
            if (_invoked.TryGetValue(typeof(T), out var value) && value) return;

            _invoked[typeof(T)] = true;
            
            var methods = typeof(T).GetMethods();
            if (methods.Length != 1)
            {
                throw new InvalidOperationException("Module contracts can only define one method");
            }

            methods.First().Invoke(GetModuleInstance<T>(), args);
        }
    }
}