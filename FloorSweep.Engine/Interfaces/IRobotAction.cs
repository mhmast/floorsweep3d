namespace FloorSweep.Api.Interfaces
{
    public interface IRobotAction
    {
        RobotActionType Type { get; }
        int Data { get; }
    }
}
