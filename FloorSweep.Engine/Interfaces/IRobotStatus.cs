namespace FloorSweep.Api.Interfaces
{
    public interface IRobotStatus
    {
        IRobotAction CurrentAction { get; }
        int DistanceToObject { get; }
    }
}
