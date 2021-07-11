using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Events;
using System.Threading.Tasks;

namespace FloorSweep.Engine.AI.Simple
{
    internal class SimpleRobotAI : ISimpleRobotAI
    {
        private readonly IEventService _eventService;
        private readonly IRobotCommandFactory _commandFactory;

        public SimpleRobotAI(IEventService eventService,IRobotCommandFactory commandFactory)
        {
            _eventService = eventService;
            _commandFactory = commandFactory;
        }
        public async Task<bool> OnStatusUpdatedAsync(IRobotStatus status)
        {
            IRobotCommand command = null;
            if(status.DistanceToObject < 10 || status.CurrentAction.Type == RobotActionType.Stopped)
            {
                command = _commandFactory.CreateTurnCommand(90);
            }
            else if(status.CurrentAction.Type != RobotActionType.Driving)
            {
                command = _commandFactory.CreateDriveCommand();
            }
            if (command != null)
            {
                await _eventService.SendRobotCommandAsync(command);
            }
            return true;
        }

        public Task ResetStatusAsync()
        => Task.CompletedTask;
    }
}
