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
            Error = sessionStatus.Error;
        }
        public string Error { get; set; }
        public DiagnosticStatus Status { get; set; }

        public static string KEY = "DiagnosticStatusData";
        public string Key => KEY;
    }
}
