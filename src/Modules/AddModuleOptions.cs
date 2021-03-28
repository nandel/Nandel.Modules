using System;
using System.Collections.Generic;

namespace Nandel.Modules
{
    public class AddModuleOptions
    {
        public IEnumerable<object> Services { get; set; }
        public IEnumerable<Type> Modules { get; set; } 
    }
}