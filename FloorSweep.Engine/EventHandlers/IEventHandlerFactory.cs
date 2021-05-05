namespace FloorSweep.Engine.EventHandlers
{
    public interface IEventHandlerFactory<T>
    {
        IEventHandler<T> GetEventHandler();
    }
}
