namespace FloorSweep.Engine.Core
{
    public interface IRobotAction
    {
        RobotActionType Type { get; }
        long Data { get; }
    }
}
