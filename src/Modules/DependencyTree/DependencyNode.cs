using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nandel.Modules
{
    public class DependencyNode : IDependencyNode
    {
        public Type ModuleType { get; }
        public DependencyTree Root { get; }
        public DependencyTree Next { get; }
        
        public bool RegistredServices { get; set; }
        public bool Initialized { get; set; }
        public bool Started { get; set; }
        public bool Stoped { get; set; }
        
        private object _module;

        public DependencyNode(Type moduleType, DependencyTree root)
        {
            ModuleType = moduleType;
            Root = root;
            Next = new DependencyTree(Root);
        }

        public void AddDependencies()
        {
            var dependencies = ModuleType.GetCustomAttributes(inherit: true)
                .OfType<DependsOnAttribute>()
                .SelectMany(x => x.ModuleTypes)
                .ToList()
                ;

            Next.Add(dependencies);
        }

        public override string ToString()
        {
            return $"{ModuleType.Name}:{GetHashCode()}";
        }

        private T GetModule<T>() where T : class
        {
            if (_module is null)
            {
                _module = Activator.CreateInstance(ModuleType);
            }

            return _module as T;
        }

        #region IDependencyNode
        
        public void RegisterServices(IServiceCollection services)
        {
            if (RegistredServices) return;
            
            RegistredServices = true;
            Next.RegisterServices(services);

            GetModule<IModule>().RegisterServices(services);
        }

        public void Initialize(IServiceProvider services)
        {
            if (Initialized) return;

            Initialized = true;
            Next.Initialize(services);
            
            GetModule<IHasInitialize>()?.Initialize(services);
        }

        public async Task StopAsync(IServiceProvider services)
        {
            if (Stoped) return;

            Stoped = true;
            Started = false;

            await Next.StopAsync(services);
            await (GetModule<IHasStop>()?.StopAsync(services) ?? Task.CompletedTask);
        }

        public async Task StartAsync(IServiceProvider services)
        {
            if (Started) return;

            Started = true;
            Stoped = false;

            await Next.StartAsync(services);
            await (GetModule<IHasStart>()?.StartAsync(services) ?? Task.CompletedTask);
        }
        
        #endregion IDependencyNode
    }
}