using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalIRServer.Scheduling;
using SignalIRServer.Services;

namespace SignalIRServer
{
    public class NewDayTask : IScheduledTask
    {
        public string Schedule => "04 20 * * *";

        private ILightsService _lightsService;
        private readonly ILogger _logger;
        
        public NewDayTask(ILogger<NewDayTask> logger, ILightsService lightsService)
        {
            _lightsService = lightsService;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task fire up");
            foreach (var lightBulb in _lightsService.GetListOfLightBullbs())
            {
                lightBulb.BulbOffTimeInMinutesPerDay = 1440;
                lightBulb.BulbOnTimeInMinutesPerDay = 0;
            }
            _logger.LogInformation("Task ends");
        }
    }
}
