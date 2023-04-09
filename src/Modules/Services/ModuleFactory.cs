using System;
using System.Collections.Generic;
using System.Linq;

namespace Nandel.Modules;

/// <summary>
/// How module objects are created
/// </summary>
public class ModuleFactory
{
    /// <summary>
    /// Default instance of factory
    /// </summary>
    public static ModuleFactory Default { get; } = new();

    /// <summary>
    /// Function on how modules are created
    /// </summary>
    public Func<Type, object> CreateInstance { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    public ModuleFactory()
    {
        CreateInstance = Activator.CreateInstance;
    }

    /// <summary>
    /// Create a factory with some available services
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public ModuleFactory(IEnumerable<object> services)
    {
        var servicesArray = services as object[] ?? services?.ToArray() ?? Array.Empty<object>();

        CreateInstance = (type) =>
        {
            var constructors = type.GetConstructors();
            foreach (var ctr in constructors)
            {
                var parameters = ctr.GetParameters();

                if (parameters.Length == 0)
                {
                    return Activator.CreateInstance(type);
                }

                if (parameters.Length > servicesArray.Length)
                {
                    continue;
                }

                var args = parameters
                    .Select(parameter => servicesArray.FirstOrDefault(x => parameter.ParameterType.IsInstanceOfType(x)))
                    .Where(arg => arg is not null)
                    .ToArray()
                    ;

                if (args.Length == parameters.Length)
                {
                    return ctr.Invoke(args);
                }
            }

            throw new InvalidOperationException($"Can't find a compatible constructor for {type}");
        };
    }
}