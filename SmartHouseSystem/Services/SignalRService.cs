﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartHouseSystem.Model;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace SmartHouseSystem.Services
{
    public class SignalRService : ISignalRService
    {
        private HubConnection _connection;
        private ConnectionState _currentConnectionState = ConnectionState.Disconnected;

        private bool _lightsListListLoaded = false;

        public bool LightsListLoaded
        {
            get => _lightsListListLoaded;
            set
            {
                _lightsListListLoaded = value;
                OnLightsListLoadedPropertyChanged();
            }
        }

        public ConnectionState CurrentConnectionState
        {
            get => _currentConnectionState;
            set
            {
                _currentConnectionState = value;
                OnCurrentConnectionStatePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler CurrentConnectionStatePropertyChanged;

        protected virtual void OnCurrentConnectionStatePropertyChanged([CallerMemberName] string propertyName = null)
        {
            CurrentConnectionStatePropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler LightsListLoadedPropertyChanged;

        protected virtual void OnLightsListLoadedPropertyChanged([CallerMemberName] string propertyName = null)
        {
            LightsListLoadedPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private  ILightService _lightService;
        public async Task ConnectionBuilder(IChartService chartService,ILightService lightService)
        {
            _lightService = lightService;

             _connection = new HubConnectionBuilder()
               .WithUrl("https://signalirserver20181021093049.azurewebsites.net/LightApp")
               //  .WithUrl("http://localhost:51690/LightApp")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

            _connection.Closed += (ex) =>
            {
                if (ex == null)
                {
                    Trace.WriteLine("Connection terminated");
                    CurrentConnectionState = ConnectionState.Disconnected;
                }
                else
                {
                    Trace.WriteLine($"Connection terminated with error: {ex.GetType()}: {ex.Message}");
                    CurrentConnectionState = ConnectionState.Faulted;
                }
            };
            
            _connection.On<bool>("InvokeStatisticsService", (isStatisticsServiceOn) =>
            {
                Debug.WriteLine("Reciving statistic data from background uwp app");
             //  chartService.ChartHandler(isStatisticsServiceOn, lightService);
            });

            _connection.On<int,bool,DateTime, string>("SendLightState", (lightID, lightStatus,dateTime, serializedLightBulbModel) =>
            {
               var lightBulbModel = JsonConvert.DeserializeObject<LightModel>(serializedLightBulbModel);
                if (lightService.LightModelList.Any(light=> light.ID==lightID))
                {
                    //                    lightService.LightModelList.First(light => light.ID == lightID).LightStatus = lightStatus;
                    //                    if (lightStatus == true)
                    //                    {
                    //                        lightService.LightModelList.First(light => light.ID == lightID).TimeOn = dateTime;
                    //                    }else if(lightStatus == false)
                    //                        lightService.LightModelList.First(light => light.ID == lightID).TimeOff = dateTime;
                    lightService.LightModelList.First(light => light.ID == lightID).BulbOffTimeInMinutesPerDay =
                        lightBulbModel.BulbOffTimeInMinutesPerDay;
                    lightService.LightModelList.First(light => light.ID == lightID).BulbOnTimeInMinutesPerDay =
                        lightBulbModel.BulbOnTimeInMinutesPerDay;
                    lightService.LightModelList.First(light => light.ID == lightID).LightStatus =
                        lightBulbModel.LightStatus;
                    lightService.LightModelList.First(light => light.ID == lightID).TimeOff =
                        lightBulbModel.TimeOff;
                    lightService.LightModelList.First(light => light.ID == lightID).TimeOn=
                        lightBulbModel.TimeOn;
                }
            });

            _connection.On<string>("SendInitialLightCollection", lightsCollection =>
            {
                Debug.WriteLine(lightsCollection);
                lightService.LightModelList = new ObservableCollection<LightModel> (JsonConvert.DeserializeObject<List<LightModel>>(lightsCollection));
                foreach (var bulb in lightService.LightModelList)
                {
                    lightService.InitNotificationOfBulbChange(bulb);
                }
                LightsListLoaded = true;
            });
           
           await ConnectAsync();
        }

        private async Task ConnectAsync()
        {
            for (var connectCount = 0; connectCount <= 3; connectCount++)
            {
                try
                {
                    await _connection.StartAsync();

                    CurrentConnectionState = ConnectionState.Connected;
                    break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Connection.Start Failed: {ex.GetType()}: {ex.Message}");

                    if (connectCount == 3)
                    {
                        CurrentConnectionState = ConnectionState.Faulted;
                        throw;
                    }
                }
                await Task.Delay(1000);
            }
        }
        
        public async Task InvokeTurnOnLight(bool isOn,int lightNumber)
        {
            await _connection.InvokeAsync("ChangeLightState", isOn, lightNumber);
            _lightService.LightModelList.First(light => light.ID == lightNumber).LightStatus = isOn;
        }
        
        public enum ConnectionState
        {
            Connected,
            Disconnected,
            Faulted
        }
    }
}
   