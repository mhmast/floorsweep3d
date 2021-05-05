using FloorSweep.Engine.Models;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Diagnostics
{
    internal class DiagnosticService : IDiagnosticsService
    {
        public async Task<bool> OnStatusUpdatedAsync(IRobotStatus status)
        {
            return true;
        }
    }
}
