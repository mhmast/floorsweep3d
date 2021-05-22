using FloorSweep.Engine.Session;

namespace FloorSweep.Engine.Diagnostics
{
    
    public interface IDiagnosticStatusData
    {
        DiagnosticStatus Status { get; }
        string Error { get; }
    }
}
