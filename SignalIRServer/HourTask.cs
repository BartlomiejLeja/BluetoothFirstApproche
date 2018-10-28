using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalIRServer.HttpClient;
using SignalIRServer.Scheduling;
using SignalIRServer.Services;

namespace SignalIRServer
{
    public class HourTask : IScheduledTask
    {
        public string Schedule => "*/3 * * * *";
        private readonly ILogger _logger;
        private ISignalRClientService _signalRClientService;

        public HourTask(ILogger<HourTask> logger, ISignalRClientService signalRClientService)
        {
            _signalRClientService = signalRClientService;
            _logger = logger;
        }
        
       public async Task ExecuteAsync(CancellationToken cancellationToken)
       {
           var httpClient = new PredictionScheduleApiClient();
           var schedule =  await httpClient.GetSchedule();

            _logger.LogInformation("HourTask fire up");
           await _signalRClientService.InvokeSendSchedulePlan(schedule);
            _logger.LogInformation("Task ends");
        }
    }
}
