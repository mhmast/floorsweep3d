using FloorSweep.Engine.Interfaces;

namespace FloorSweep.Engine.Repositories
{
    internal class Session : ISession
    {
        public string Id { get; }

        public Session(string id)
        {
            Id = id;
        }
    }
}
