using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalIRServer.Services
{
    public class ChartService
    {
        private static Timer timer;
        private DateTime _firstDate;

        public ChartService(ILightsService lightService)
        {
//            _firstDate = DateTime.Now;
//            ChartHandler(true, lightService);
//
//            var timerState = new TimerState { Counter = 0 };
//
//            timer = new Timer(
//                callback: new TimerCallback(ChartHandler),
//                state: timerState,
//                dueTime: 1000,
//                period: 2000);

        }

        public void ChartHandler(object timerStatus)
        {
           


        }
    }
}
