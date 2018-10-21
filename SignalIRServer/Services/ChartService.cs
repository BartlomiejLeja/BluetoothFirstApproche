using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalIRServer.Services
{
    public class ChartService
    {
        private DateTime _firstDate;

//        public ChartService(LightsService lightService)
//        {
//            _firstDate = DateTime.Now;
//            ChartHandler(true, lightService);
//        }

//        public void ChartHandler(bool isStatisticsServiceOn, LightsService lightService)
//        {
//            var delay = TimeSpan.FromSeconds(30);
//
//            var periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(
//                (source) =>
//                {
//                    if (_firstDate.Day == DateTime.Now.Day)
//                    {
//                        foreach (var light in lightService.LightModelList)
//                        {
//                            if (light.LightStatus)
//                            {
//                                light.BulbOnTimeInMinutesPerDay--;
//                                light.BulbOffTimeInMinutesPerDay++;
//                                Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {light.BulbOffTimeInMinutesPerDay} " +
//                                                $"and bulbOffTimeInMinutes changed, new value {light.BulbOnTimeInMinutesPerDay} date: {DateTime.Now}");
//                            }
//                        }
//                    }
//                    else
//                    {
//                        foreach (var light in lightService.LightModelList)
//                        {
//                            light.BulbOnTimeInMinutesPerDay = 1439;
//                            light.BulbOffTimeInMinutesPerDay = 1;
//                        }
//                        _firstDate = DateTime.Now;
//                    }
//                }, delay);
//        }
    }
}
