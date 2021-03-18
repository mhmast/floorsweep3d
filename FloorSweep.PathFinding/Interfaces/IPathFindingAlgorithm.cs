namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPathFindingAlgorithm
    {
        IPathFindingSession CreateSession(MapData data);
    }
}
