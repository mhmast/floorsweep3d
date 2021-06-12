using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Session;
using FloorSweep.Math;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Events
{
    public interface IEventService
    {
        Task SendMatrixInitAsync(string name,Mat m,bool isBinary);
        Task SendMatrixUpdateAsync(string key, int row, int col, double value);
        Task SendRobotCommandAsync(IRobotCommand command);
        Task SendRobotStatusUpdateAsync(IRobotStatus status);
        Task SendSessionUpdatedAsync(ISession session);
    }
}