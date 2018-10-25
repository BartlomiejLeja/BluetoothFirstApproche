using System;
using System.Diagnostics;
using Windows.System.Threading;

namespace SmartHouseSystem.Services
{
    public class ChartService :IChartService
    {
        public void ChartHandler( ILightService lightService)
        {
                var delay = TimeSpan.FromSeconds(60);

                var periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(
                    (source) =>
                    {
                                foreach (var light in lightService.LightModelList)
                                {
                                    if (light.LightStatus)
                                    {
                                        light.BulbOnTimeInMinutesPerDay++;
                                        light.BulbOffTimeInMinutesPerDay--;
                                        Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {light.BulbOffTimeInMinutesPerDay} " +
                                                        $"and bulbOffTimeInMinutes changed, new value {light.BulbOnTimeInMinutesPerDay} date: {DateTime.Now}");
                                    }
                                }
                    }, delay);
            } 
    }
}
