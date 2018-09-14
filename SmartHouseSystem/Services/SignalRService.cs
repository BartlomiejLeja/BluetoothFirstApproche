using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartHouseSystem.Model;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.System.Threading;

namespace SmartHouseSystem.Services
{
    public class SignalRService : ISignalRService
    {
        private HubConnection connection;
        private ConnectionState _connectionState1 = ConnectionState.Disconnected;

        public ConnectionState ConnectionState1
        {
            get => _connectionState1;
            set
            {
                _connectionState1 = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<StatusModel1> ListOfstatus { get; set; }

        private  ILightService _lightService;
        public async Task ConnectionBuilder(IChartService chartService,ILightService lightService)
        {
            _lightService = lightService;
             connection = new HubConnectionBuilder()
                .WithUrl("https://signalirserver20180827052120.azurewebsites.net/message")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

            connection.Closed += (ex) =>
            {
                if (ex == null)
                {
                    Trace.WriteLine("Connection terminated");
                    ConnectionState1 = ConnectionState.Disconnected;
                }
                else
                {
                    Trace.WriteLine($"Connection terminated with error: {ex.GetType()}: {ex.Message}");
                    ConnectionState1 = ConnectionState.Faulted;
                }
            };

            connection.On<bool>("CheckStatusOfLight", async data=>
            {
//                Debug.WriteLine("Cheking status in uwp app");
//                var isOn= await wiFiService.CheckStatusOfLight();
//                await InvokeSendStatusMethod(isOn);
                await Task.Delay(3000);
            });

            connection.On<bool,int>("SendLightStatisticData", (lightStatus, lightNumber )=>
            {
                Debug.WriteLine("Reciving statistic data from background uwp app");
                chartService.ChartHandler(lightStatus, lightNumber);
            });

            connection.On<int,bool>("SendLightState", (lightID, lightStatus) =>
            {
                //list will be asigne to some avible forr all components list 
                if (lightService.StatusModels.All(light => light.ID != lightID) || lightService.StatusModels.Count ==0)
                {
                    lightService.StatusModels.Add(new StatusModel1(lightID, lightStatus));
                    lightService.InitNotificationOfChange(lightID);
                }
                else if (lightService.StatusModels.Any(light=> light.ID==lightID))
                {
                    lightService.StatusModels.First(light => light.ID == lightID).LightStatus = lightStatus;
                }
            });
            await ConnectAsync();
        }

        private async Task ConnectAsync()
        {
            for (int connectCount = 0; connectCount <= 3; connectCount++)
            {
                try
                {
                    await connection.StartAsync();

                    ConnectionState1 = ConnectionState.Connected;
                    break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Connection.Start Failed: {ex.GetType()}: {ex.Message}");

                    if (connectCount == 3)
                    {
                        ConnectionState1 = ConnectionState.Faulted;
                        throw;
                    }
                }

                await Task.Delay(1000);
            }
        }


        public async Task InvokeSendStaticticData(bool status)
        {
            await connection.InvokeAsync("SendLightStatisticData", status);
        }
        
        //GOOD
        public async Task InvokeTurnOnLight(bool isOn,int lightNumber)
        {
            await connection.InvokeAsync("ChangeLightState", isOn, lightNumber);
            _lightService.StatusModels.First(light => light.ID == lightNumber).LightStatus = isOn;
        }

        public async Task InvokeCheckStatusOfLights(bool x)
        {
            await connection.InvokeAsync("CheckStatusOfLights", x);
        }
        public enum ConnectionState
        {
            Connected,
            Disconnected,
            Faulted
        }
    }
}
    

