using System;

namespace FloorSweep.Engine.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
