using FloorSweep.Engine.Core;

namespace FloorSweep.Engine.Diagnostics
{
    public interface IDiagnosticStatusData
    {
        double AvgSpeedMmPerSecond { get; }
        int AvgSpeedPixelsPerSecond { get; }
        string Error { get;  }
        IRobotStatus LastReceivedStatus { get; }
        DiagnosticStatus Status { get; }
    }
}