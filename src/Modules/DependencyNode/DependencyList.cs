using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    /// <summary>
    /// List of DependencyNodes
    /// </summary>
    public class DependencyList : List<IDependencyNode>, IDependencyNode
    {
        public IDependencyNode Root { get; }
        
        private readonly ModuleFactory _factory;

        public DependencyList(ModuleFactory factory, params Type[] modulesTypes)
        {
            _factory = factory;
            Root = this;
            
            AddRange(modulesTypes);
        }

        public DependencyList(ModuleFactory factory, IDependencyNode root)
        {
            _factory = factory;
            Root = root;
        }

        public void Add(Type moduleType)
        {
            var node = Root.FindNode(moduleType);
            
            if (node is null)
            {
                node = new DependencyNode(_factory, moduleType, Root);
                
                Add(node);
                ((DependencyNode) node).AddDependencies();
            }
            else
            {
                Add(node);
            }
        }

        public void AddRange(IEnumerable<Type> modulesType)
        {
            foreach (var moduleType in modulesType)
            {
                Add(moduleType);
            }
        }

        public IDependencyNode FindNode(Type moduleType)
        {
            foreach (var node in this)
            {
                var found = node.FindNode(moduleType);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
        
        public void RegisterServices(IServiceCollection services)
        {
            foreach (var node in this)
            {
                node.RegisterServices(services);
            }
        }

        public void Initialize(IServiceProvider services)
        {
            foreach (var node in this)
            {
                node.Initialize(services);
            }
        }
        
        public async Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            foreach (var node in this)
            {
                await node.StartAsync(services, cancellationToken);
            }
        }

        public async Task StopAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            foreach (var node in this)
            {
                await node.StopAsync(services, cancellationToken);
            }
        }
    }
}