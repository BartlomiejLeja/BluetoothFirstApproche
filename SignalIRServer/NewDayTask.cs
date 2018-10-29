using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalIRServer.Hubs;
using SignalIRServer.Scheduling;
using SignalIRServer.Services;

namespace SignalIRServer
{
    public class NewDayTask : IScheduledTask
    {
        public string Schedule => "1 0 * * *";

        private readonly ILightsService _lightsService;
        private readonly ILogger _logger;
        private readonly IHubContext<Broadcaster> _hubContext;

        public NewDayTask(ILogger<NewDayTask> logger, ILightsService lightsService, IHubContext<Broadcaster> hubContext)
        {
            _hubContext = hubContext;
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
