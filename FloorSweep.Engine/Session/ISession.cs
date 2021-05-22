namespace FloorSweep.Engine.Session
{
    public interface ISession
    {
        string Id { get; }

        T GetObject<T>();

        void SetObject<T>(T obj);
    }
}
