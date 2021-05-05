namespace FloorSweep.Engine.StatusHandlers
{
    public interface IStatusUpdateHandlerFactory<T>
    {
        IStatusUpdateHandler<T> GetStatusUpdateHandler();
    }
}
