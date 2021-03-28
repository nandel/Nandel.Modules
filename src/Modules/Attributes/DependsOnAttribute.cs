using System;
using System.Collections.Generic;

namespace Nandel.Modules
{
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(params Type[] moduleTypes)
        {
            ModuleTypes = moduleTypes;
        }

        public ICollection<Type> ModuleTypes { get; protected set; }
    }
}