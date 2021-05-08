using FloorSweep.Engine.Session;
using System.Collections.Generic;

namespace FloorSweep.Api.Repositories
{
    internal class Session : Dictionary<string, object>, ISession
    {
        public string Id { get; }

        public Session(string id)
        {
            Id = id;
        }

        public T GetObject<T>(string key) 
        => (T)(ContainsKey(key) ? this[key] : default(T));

        public void SetObject(string key,object obj)
        {
            if (!ContainsKey(key))
            {
                Add(key, obj);
            }
            else
            {
                this[key] = obj;
            }
        }
    }
}
