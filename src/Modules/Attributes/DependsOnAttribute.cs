using System;
using System.Collections.Generic;
using System.Linq;

namespace Nandel.Modules;

/// <summary>
/// Define what modules the current module class depends on
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="moduleTypes"></param>
    public DependsOnAttribute(params Type[] moduleTypes)
    {
        ModuleTypes = moduleTypes;
    }

    /// <summary>
    /// List of modules that the current class depends on
    /// </summary>
    public ICollection<Type> ModuleTypes { get; }
    
    /// <summary>
    /// Find the dependencies of a module using this attribute
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public static List<Type> FindDependencies(Type moduleType)
    {
        var dependencies = moduleType
            .GetCustomAttributes(inherit: false)
            .Where(x => x is DependsOnAttribute)
            .Cast<DependsOnAttribute>()
            .SelectMany(x => x.ModuleTypes)
            .Distinct()
            .ToList();

        return dependencies;
    }
}
