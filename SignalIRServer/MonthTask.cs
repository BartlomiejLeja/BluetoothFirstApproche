using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalIRServer.HttpClient;
using SignalIRServer.Scheduling;

namespace SignalIRServer
{
    public class MonthTask : IScheduledTask
    {
        public string Schedule => "* * */1 * *";
        private readonly ILogger _logger;

        public MonthTask(ILogger<MonthTask> logger)
        {
            _logger = logger;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var httpClient = new PredictionScheduleApiClient();
            await httpClient.Train();
        }
    }
}
