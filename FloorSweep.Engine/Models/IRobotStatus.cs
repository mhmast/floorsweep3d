namespace FloorSweep.Engine.Models
{
    public interface IRobotStatus
    {
        IRobotAction CurrentAction { get; }
        int DistanceToObject { get; }
    }
}
