using System;
using System.Collections.Generic;

namespace Nandel.Modules
{
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
    }
}