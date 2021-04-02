using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.Engine.Interfaces
{
    public interface ISession
    {
        string Id { get; }
        IPathFindingSession PathFindingSession
        {
            get;
        }
    }
}
