using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Diagnostics
{
    internal class DiagnosticService : IDiagnosticService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IRobotCommandFactory _robotCommandFactory;
        private readonly IEventService _eventService;

        public DiagnosticService(ISessionRepository sessionRepository, IRobotCommandFactory robotCommandFactory, IEventService eventService)
        {
            _sessionRepository = sessionRepository;
            _robotCommandFactory = robotCommandFactory;
            _eventService = eventService;
        }

        public async Task<bool> OnStatusUpdatedAsync(IRobotStatus status)
        {
            var diagnosticStatus = await EnsureDiagnosticStatusData();
            await DetermineNextStatusAsync(diagnosticStatus,status);
            return diagnosticStatus.Status != DiagnosticStatus.Done;
        }

        private Task DetermineNextStatusAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        => diagnosticStatus.Status switch
        {
            DiagnosticStatus.Unknown => StartDiagnosticSessionAsync(diagnosticStatus),
            DiagnosticStatus.TestingTurn => throw new NotImplementedException(),
            DiagnosticStatus.TestingDrive => throw new NotImplementedException(),
            DiagnosticStatus.TestingStop => throw new NotImplementedException(),
            DiagnosticStatus.Done => throw new NotImplementedException()
        };

        private async Task StartDiagnosticSessionAsync(DiagnosticStatusData diagnosticStatus)
        {
            diagnosticStatus.Status = DiagnosticStatus.TestingTurn;
            await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateTurnCommand(180));
        }

        private async Task<DiagnosticStatusData> EnsureDiagnosticStatusData()
        {
            var sessionStatus = await _sessionRepository.GetObjectAsync<IDiagnosticStatusData>();
            if(sessionStatus  == null)
            {
                var status = new DiagnosticStatusData();
                await _sessionRepository.SaveObjectAsync(status);
                return status;
            }
            return new DiagnosticStatusData(sessionStatus);
        }
    }
}
