using FloorSweep.Engine.Core;
using FloorSweep.Engine.EventHandlers;

namespace FloorSweep.Engine.AI.Simple
{
    internal interface ISimpleRobotAI : IEventHandlerDecorator<IRobotStatus>
    {
    }
}