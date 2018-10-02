using System;
using System.Diagnostics;
using Windows.System.Threading;

namespace SmartHouseSystem.Services
{
    public class ChartService :IChartService
    {
        private DateTime _firstDate;
        
        public ChartService()
        {
            _firstDate = DateTime.Now;
        }
        
        public void ChartHandler(bool isStatisticsServiceOn, ILightService lightService)
        {
                var delay = TimeSpan.FromSeconds(30);

                var periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(
                    (source) =>
                    {
                            if (_firstDate.Day == DateTime.Now.Day)
                            {
                                foreach (var light in lightService.LightModelList)
                                {
                                    if (light.LightStatus)
                                    {
                                        light.BulbOnTimeInMinutesPerDay--;
                                        light.BulbOffTimeInMinutesPerDay++;
                                        Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {light.BulbOffTimeInMinutesPerDay} " +
                                                        $"and bulbOffTimeInMinutes changed, new value {light.BulbOnTimeInMinutesPerDay} date: {DateTime.Now}");
                                    }
                                }
                            }
                            else
                            {
                                foreach (var light in lightService.LightModelList)
                                {
                                    light.BulbOnTimeInMinutesPerDay = 1439;
                                    light.BulbOffTimeInMinutesPerDay = 1;
                                }
                                _firstDate = DateTime.Now;
                            }
                    }, delay);
            } 
    }
}
