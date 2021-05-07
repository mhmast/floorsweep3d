namespace FloorSweep.Engine.Diagnostics
{
    internal class DiagnosticStatusData : IDiagnosticStatusData
    {
        
        public DiagnosticStatusData()
        {

        }
        public DiagnosticStatusData(IDiagnosticStatusData sessionStatus)
        {
            Status = sessionStatus.Status;
        }

        public DiagnosticStatus Status { get; set; }
    }
}
