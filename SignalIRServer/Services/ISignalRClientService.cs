using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalIRServer.Services
{
    public class ISignalRClientService
    {
//        private HubConnection _connection;
//
//        public async Task Connect(LightsService lightsService, MQTTServer mQTTServer, SchedulePlanService schedulePlanService)
//        {
//            _connection = new HubConnectionBuilder()
//                .WithUrl("https://signalirserver20181021093049.azurewebsites.net/LightApp")
//                .WithConsoleLogger(LogLevel.Trace)
//                .Build();
//
//            _connection.On<bool, int>("ChangeLightState", async (lightStatus, lightNumber) =>
//            {
//                Debug.WriteLine("You turn on light");
//
//                var statusOffLight = lightStatus ? "on" : "off";
//                var payLoad = $"light/{lightNumber}/{statusOffLight}";
//                await mQTTServer.PublishMessage(payLoad);
//            });
//
//            _connection.On<string>("SendInitialLightCollection", lightsCollection =>
//            {
//                Debug.WriteLine(lightsCollection);
//                //   lightsService.LightModelList = new List<LightModel>(JsonConvert.DeserializeObject<List<LightModel>>(lightsCollection));
//            });
//
//            _connection.On<string>("SendSchedulePlan", schedulePlanJson =>
//            {
//                Debug.WriteLine("SendSchedulePlan");
//                if (true)
//                {
//                    schedulePlanService.SmartSwitchingLights(mQTTServer, schedulePlanJson);
//                }
//
//            });
//            await _connection.StartAsync();
//        }
    }
}
