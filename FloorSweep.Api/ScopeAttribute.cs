using System;

namespace FloorSweep.Api
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false)]
    public class ScopeAttribute : Attribute
    {
        public ScopeAttribute(params string[] scopes)
        {
            Scopes = scopes;
        }

        public string[] Scopes { get; }
    }
}
