
using DiagnosticStatus2 = FloorSweep.Engine.Diagnostics.DiagnosticStatus;
using System;
using FloorSweep.Engine.Diagnostics;

namespace FloorSweep.Api.Hubs.Dtos
{
    internal class DiagnosticStatusUpdateDto
    {
        public DiagnosticStatusUpdateDto(IDiagnosticStatusData data)
        {
            Status = ConvertToDtoStatus(data.Status);
            Error = data.Error;
        }

        private static DiagnosticStatus ConvertToDtoStatus(DiagnosticStatus2 status)
        => status switch
        {
            DiagnosticStatus2.Unknown => DiagnosticStatus.Unknown,
            DiagnosticStatus2.TestingDrive => DiagnosticStatus.TestingDrive,
            DiagnosticStatus2.TestingStop => DiagnosticStatus.TestingStop,
            DiagnosticStatus2.TestingTurn => DiagnosticStatus.TestingTurn,
            DiagnosticStatus2.DeterminingSpeed => DiagnosticStatus.DeterminingSpeed,
            DiagnosticStatus2.Done => DiagnosticStatus.Done,
            _ => throw new NotImplementedException(),
        };
        public DiagnosticStatus Status { get; private set; }
        public string Error { get; private set; }
    }
}
