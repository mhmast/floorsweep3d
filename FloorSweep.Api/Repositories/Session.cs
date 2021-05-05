using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;

namespace FloorSweep.Api.Repositories
{
    internal class Session : Dictionary<Type, object>, ISession
    {
        public string Id { get; }

        public Session(string id)
        {
            Id = id;
        }

        public T GetObject<T>() where T : class
        => (ContainsKey(typeof(T)) ? this[typeof(T)] : default(T)) as T;

        public void SetObject<T>(T obj) where T : class
        {
            if (!ContainsKey(typeof(T)))
            {
                Add(typeof(T), obj);
            }
            else
            {
                this[typeof(T)] = obj;
            }
        }
    }
}
