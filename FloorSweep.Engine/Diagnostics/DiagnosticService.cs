using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Config;
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
        private readonly IMapConfiguration _mapConfig;

        public DiagnosticService(
            ISessionRepository sessionRepository,
            IRobotCommandFactory robotCommandFactory,
            IEventService eventService,
            IMapConfiguration mapConfig)
        {
            _sessionRepository = sessionRepository;
            _robotCommandFactory = robotCommandFactory;
            _eventService = eventService;
            _mapConfig = mapConfig;
        }

        public async Task<bool> OnStatusUpdatedAsync(IRobotStatus status)
        {
            var diagnosticStatus = await EnsureDiagnosticStatusData();
            await DetermineNextStatusAsync(diagnosticStatus, status);
            await _sessionRepository.SaveObjectAsync(diagnosticStatus);
            await _eventService.SendDiagnosticStatusUpdatedAsync(diagnosticStatus);
            return diagnosticStatus.Status != DiagnosticStatus.Done;
        }

        private Task DetermineNextStatusAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        => diagnosticStatus.Status switch
        {
            DiagnosticStatus.Unknown => TestTurnAsync(diagnosticStatus),
            DiagnosticStatus.TestingTurn => VerifyResultAndTestDriveAsync(diagnosticStatus, status),
            DiagnosticStatus.TestingDrive => VerifyResultAndTestStopAsync(diagnosticStatus, status),
            DiagnosticStatus.TestingStop => VerifyResultAndTestSpeedAsync(diagnosticStatus, status),
            DiagnosticStatus.DeterminingSpeed => VerifyResultAndDoneAsync(diagnosticStatus, status),
            DiagnosticStatus.Done => Task.CompletedTask,
            _ => throw new NotImplementedException()
        };

        private Task VerifyResultAndDoneAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        {

            var lastMessage = diagnosticStatus.LastReceivedStatus;
            var traveledDistanceInMm = lastMessage.DistanceToObject - status.DistanceToObject;
            var elapsedTimeSeconds = (status.StatusDate - lastMessage.StatusDate).TotalSeconds;
            diagnosticStatus.AvgSpeedMmPerSecond = traveledDistanceInMm / elapsedTimeSeconds;
            diagnosticStatus.AvgSpeedPixelsPerSecond = (int)(diagnosticStatus.AvgSpeedMmPerSecond * _mapConfig.PixelsPerMM);
            diagnosticStatus.Status = DiagnosticStatus.Done;

            return Task.CompletedTask;
        }

        private static Task VerifyResultAndTestSpeedAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        {
            if (status.CurrentAction.Type == RobotActionType.Stopped)
            {
                diagnosticStatus.LastReceivedStatus = status;
                diagnosticStatus.Status = DiagnosticStatus.DeterminingSpeed;
            }
            else
            {
                diagnosticStatus.Error = $"Unexpected robot action type {status.CurrentAction.Type}";
            }
            return Task.CompletedTask;
        }



        private async Task VerifyResultAndTestStopAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        {
            if (status.CurrentAction.Type == RobotActionType.Driving)
            {
                diagnosticStatus.Status = DiagnosticStatus.TestingStop;
                await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateStopCommand());
                return;
            }
            diagnosticStatus.Error = $"Unexpected robot action type {status.CurrentAction.Type}";
        }

        private async Task VerifyResultAndTestDriveAsync(DiagnosticStatusData diagnosticStatus, IRobotStatus status)
        {
            if (status.CurrentAction.Type == RobotActionType.Turned)
            {
                diagnosticStatus.Status = DiagnosticStatus.TestingDrive;
                await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateDriveCommand());
                return;
            }
            diagnosticStatus.Error = $"Unexpected robot action type {status.CurrentAction.Type}";
        }

        private async Task TestTurnAsync(DiagnosticStatusData diagnosticStatus)
        {
            diagnosticStatus.Status = DiagnosticStatus.TestingTurn;
            await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateTurnCommand(180));
        }

        private async Task<DiagnosticStatusData> EnsureDiagnosticStatusData()
        => await _sessionRepository.GetObjectAsync<DiagnosticStatusData>()?? new DiagnosticStatusData();

        public async Task ResetStatusAsync()
        {
            var status = new DiagnosticStatusData();
            await _sessionRepository.SaveObjectAsync(status);
            await _eventService.SendDiagnosticStatusUpdatedAsync(status);
        }
    }
}
