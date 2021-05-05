namespace FloorSweep.Engine.Session
{
    public interface ISession
    {
        string Id { get; }

        T GetObject<T>() where T : class;

        void SetObject<T>(T obj) where T : class;
    }
}
