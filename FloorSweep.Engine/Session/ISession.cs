namespace FloorSweep.Engine.Session
{
    public interface ISession
    {
        string Id { get; }

        T GetObject<T>(string key);

        void SetObject(string key,object obj);
    }
}
