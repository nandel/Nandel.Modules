using System;
using System.Collections.Generic;

namespace Nandel.Modules
{
    /// <summary>
    /// Options to add a module
    /// </summary>
    public class AddModuleOptions
    {
        /// <summary>
        /// Services to initialize Module classes
        /// </summary>
        public IEnumerable<object> Services { get; set; }
        
        /// <summary>
        /// Types of Modules you want configure
        /// </summary>
        public IEnumerable<Type> Modules { get; set; } 
    }
}