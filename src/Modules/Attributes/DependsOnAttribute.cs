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
        public DependsOnAttribute(params Type[] moduleTypes)
        {
            ModuleTypes = moduleTypes;
        }

        public ICollection<Type> ModuleTypes { get; protected set; }
    }
}