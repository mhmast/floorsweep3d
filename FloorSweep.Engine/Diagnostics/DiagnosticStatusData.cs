using FloorSweep.Engine.Core;
using FloorSweep.Engine.Session;

namespace FloorSweep.Engine.Diagnostics
{
    [SessionSaveable]
    internal class DiagnosticStatusData : IRobotMeta
    {

        public DiagnosticStatusData()
        {

        }
    
        public string Error { get; set; }
        public DiagnosticStatus Status { get; set; }

        public IRobotStatus LastReceivedStatus { get; set; }
        public double AvgSpeedMmPerSecond { get; internal set; }
        public int AvgSpeedPixelsPerSecond { get; internal set; }
    }
}
