namespace FloorSweep.Engine.Models
{
    public interface IRobotAction
    {
        RobotActionType Type { get; }
        int Data { get; }
    }
}
