using System;

namespace FloorSweep.Engine.Session
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SessionSaveableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
