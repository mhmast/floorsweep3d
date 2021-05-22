using FloorSweep.Engine.Session;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FloorSweep.Api.Repositories
{
    internal class Session : Dictionary<string, object>, ISession
    {
        public string Id { get; }

        public Session(string id)
        {
            Id = id;
        }

        public T GetObject<T>()
        {
            var key = GetKey<T>();
            return (T)(ContainsKey(key) ? this[key] : default(T));
        }

        private static string GetKey<T>()
        {
            var a = typeof(T).GetCustomAttributes(false).OfType<SessionSaveableAttribute>().FirstOrDefault();
            if(a?.Name == null)
            {
                return typeof(T).Name;
            }
            return a.Name;
        }

        public void SetObject<T>(T obj)
        {
            var key = GetKey<T>();

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
