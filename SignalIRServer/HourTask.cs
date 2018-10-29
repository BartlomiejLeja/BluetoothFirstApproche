using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalIRServer.HttpClient;
using SignalIRServer.Hubs;
using SignalIRServer.Scheduling;
using SignalIRServer.Services;

namespace SignalIRServer
{
    public class HourTask : IScheduledTask
    {
        public string Schedule => "*/5 * * * *";
        private readonly ILogger _logger;
       
        private readonly IHubContext<Broadcaster> _hubContext;
        private readonly ILightsService _lightsService;
        public HourTask(ILogger<HourTask> logger,
            IHubContext<Broadcaster> hubContext, ILightsService lightsService)
        {
            _hubContext = hubContext;
            _lightsService = lightsService;
            _logger = logger;
        }
        
       public async Task ExecuteAsync(CancellationToken cancellationToken)
       {
           var httpClient = new PredictionScheduleApiClient();
           _logger.LogInformation("HourTask fire up");
           var dateNow = DateTime.Now;
            foreach (var lightBulb in _lightsService.GetListOfLightBullbs())
           {
               var schedule = await httpClient.GetSchedule(lightBulb.ID, dateNow.Month,
                   (int)dateNow.DayOfWeek, (dateNow.Hour * 60), (dateNow.Hour * 60 + 60));
               await _hubContext.Clients.All.SendAsync("SendSchedulePlan", schedule);
            }
           
            _logger.LogInformation("Task ends");
        }
    }
}
