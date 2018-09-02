using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartHouseSystem.Model;
using System.Linq;

namespace SmartHouseSystem.Services
{
    public class SignalRService : ISignalRService
    {
        private HubConnection connection;
     
        public List<StatusModel1> ListOfstatus { get; set; }


        public async Task Connect(IWiFiService wiFiService,IChartService chartService,ILightService lightService)
        {
        
        connection = new HubConnectionBuilder()
                .WithUrl("https://signalirserver20180827052120.azurewebsites.net/message")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();
            
            connection.On<bool>("CheckStatusOfLight", async data=>
            {
//                Debug.WriteLine("Cheking status in uwp app");
//                var isOn= await wiFiService.CheckStatusOfLight();
//                await InvokeSendStatusMethod(isOn);
            });

            connection.On<bool,int>("SendLightStatisticData", (lightStatus, lightNumber )=>
            {
                Debug.WriteLine("Reciving statistic data from background uwp app");
                chartService.ChartHandler(lightStatus, lightNumber);
            });

            connection.On<int,bool>("SendLightState", (lightID, lightStatus) =>
            {
                //list will be asigne to some avible forr all components list 
                if (lightService.StatusModels.All(light => light.ID != lightID))
                {
                    lightService.StatusModels.Add(new StatusModel1(lightID, lightStatus));
                }
            });

            await connection.StartAsync();
        }
        

        public async Task InvokeSendStaticticData(bool status)
        {
            await connection.InvokeAsync("SendLightStatisticData", status);
        }
        
        //GOOD
        public async Task InvokeTurnOnLight(bool isOn,int lightNumber)
        {
            await connection.InvokeAsync("ChangeLightState", isOn, lightNumber);
        }

        public async Task InvokeCheckStatusOfLights(bool x)
        {
            await connection.InvokeAsync("CheckStatusOfLights", x);
        }
        
    }
}
    

