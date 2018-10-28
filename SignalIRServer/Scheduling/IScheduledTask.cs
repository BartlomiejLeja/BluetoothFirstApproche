using System.Threading;
using System.Threading.Tasks;

namespace SignalIRServer.Scheduling
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
