namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IUpdateableResource
    {
        void Update<T>(T resource) where T : struct;

    }
}
