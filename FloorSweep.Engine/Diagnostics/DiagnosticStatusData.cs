using FloorSweep.Engine.Session;

namespace FloorSweep.Engine.Diagnostics
{
    [SessionSaveable]
    internal class DiagnosticStatusData : IDiagnosticStatusData
    {

        public DiagnosticStatusData()
        {

        }
        public DiagnosticStatusData(IDiagnosticStatusData sessionStatus)
        {
            Status = sessionStatus.Status;
            Error = sessionStatus.Error;
        }
        public string Error { get; set; }
        public DiagnosticStatus Status { get; set; }

    }
}
