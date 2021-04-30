namespace FloorSweep.Engine.Interfaces
{
    public interface IStatusUpdateHandlerFactory<T>
    {
        IStatusUpdateHandler<T> GetStatusUpdateHandler();
    }
}
