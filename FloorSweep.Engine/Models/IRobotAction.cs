namespace FloorSweep.Engine.Models
{
    public interface IRobotAction
    {
        RobotActionType Type { get; }
        long Data { get; }
    }
}
