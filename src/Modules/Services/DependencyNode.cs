using System;
using System.Collections.Generic;
using System.Linq;

namespace Nandel.Modules;

public class DependencyNode
{
    public DependencyNode(Type moduleType, ModuleFactory moduleFactory)
    {
        Controller = new DependencyController(moduleType, moduleFactory);
        ModuleType = moduleType;
        ModuleFactory = moduleFactory;
        Root = this;
        Dependencies = new List<DependencyNode>();
        
        DependsOnAttribute.FindDependencies(moduleType)
            .ForEach(AddDependencyNode);
    }

    private DependencyNode(Type moduleType, ModuleFactory moduleFactory, DependencyNode root)
    {
        Controller = root.FindControllerByModuleType(moduleType) ?? new DependencyController(moduleType, moduleFactory);
        ModuleType = moduleType;
        ModuleFactory = moduleFactory;
        Root = root;
        Dependencies = root.FindDependenciesByModuleType(moduleType);

        if (Dependencies is null)
        {
            Dependencies = new List<DependencyNode>();
            DependsOnAttribute.FindDependencies(moduleType)
                .ForEach(AddDependencyNode);
        }
    }
    
    public DependencyController Controller { get; }
    public Type ModuleType { get; }
    public ModuleFactory ModuleFactory { get; }
    public DependencyNode Root { get; }
    public IList<DependencyNode> Dependencies { get; }

    public void AddDependencyNode(Type moduleType)
    {
        Dependencies.Add(new DependencyNode(moduleType, ModuleFactory, Root));
    }

    public IEnumerable<DependencyNode> AsEnumerable()
    {
        return AsEnumerableRecursively(new List<Type>());
    }

    public DependencyNode FindNodeByModuleType(Type moduleType)
    {
        return AsEnumerable().FirstOrDefault(x => x.ModuleType == moduleType);
    }

    public DependencyController FindControllerByModuleType(Type moduleType)
    {
        return FindNodeByModuleType(moduleType)?.Controller;
    }
    
    public IList<DependencyNode> FindDependenciesByModuleType(Type moduleType)
    {
        return FindNodeByModuleType(moduleType)?.Dependencies;
    }

    public void ConfigureServices<T>(T serviceCollection)
    {
        AsEnumerable()
            .ToList()
            .ForEach(x => x.Controller.ConfigureServices(serviceCollection));
    }

    private IEnumerable<DependencyNode> AsEnumerableRecursively(ICollection<Type> visited)
    {
        if (visited.Contains(ModuleType))
        {
            yield break;
        }
        
        visited.Add(ModuleType);
        
        foreach (var dependencyNode in Dependencies)
        {
            foreach (var dependency in dependencyNode.AsEnumerableRecursively(visited))
            {
                yield return dependency;
            }
        }
        
        yield return this;
    }
}