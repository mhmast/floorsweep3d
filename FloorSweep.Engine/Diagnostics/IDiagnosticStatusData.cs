using FloorSweep.Engine.Session;

namespace FloorSweep.Engine.Diagnostics
{
    public interface IDiagnosticStatusData : IKeyable
    {
        DiagnosticStatus Status { get; }
        string Error { get; }
    }
}
