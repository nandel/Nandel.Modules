using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public class DependencyTree : IDependencyNode
    {
        public DependencyTree(params Type[] modulesTypes)
        {
            Root = this;
            Add(modulesTypes);
        }

        public DependencyTree(DependencyTree root)
        {
            Root = root;
        }

        public ICollection<DependencyNode> Nodes { get; } = new List<DependencyNode>();
        public DependencyTree Root { get; }

        public void Add(Type moduleType)
        {
            var node = Root.FindNode(moduleType);
            
            if (node is null)
            {
                node = new DependencyNode(moduleType, Root);
                
                Nodes.Add(node);
                node.AddDependencies();
            }
            else
            {
                Nodes.Add(node);
            }
        }

        public void Add(IEnumerable<Type> modulesType)
        {
            foreach (var moduleType in modulesType)
            {
                Add(moduleType);
            }
        }
        
        public DependencyNode FindNode(Type moduleType)
        {
            foreach (var node in Nodes)
            {
                if (moduleType == node.ModuleType)
                {
                    return node;
                }
                
                var next = node.Next.FindNode(moduleType);
                if (next != null)
                {
                    return next;
                }
            }

            return null;
        }
        
        #region IDependencyNode
        
        public void RegisterServices(IServiceCollection services)
        {
            foreach (var node in Nodes)
            {
                node.RegisterServices(services);
            }
        }

        public void Initialize(IServiceProvider services)
        {
            foreach (var node in Nodes)
            {
                node.Initialize(services);
            }
        }
        
        public async Task StartAsync(IServiceProvider services)
        {
            foreach (var node in Nodes)
            {
                await node.StartAsync(services);
            }
        }

        public async Task StopAsync(IServiceProvider services)
        {
            foreach (var node in Nodes)
            {
                await node.StopAsync(services);
            }
        }
        
        #endregion
    }
}