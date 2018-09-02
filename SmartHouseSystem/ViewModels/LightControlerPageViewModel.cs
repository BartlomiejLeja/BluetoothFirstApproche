using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System.Diagnostics;
using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel : ViewModelBase
    {
        private IWiFiService _wiFiService;
        private IChartService _chartService;
        private ISignalRService _signalRService;

        private static string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static string lightOff = "ms-appx:///Images/lightTurnOff.jpg";

        public DelegateCommand<string> ChangeLightState { get; private set; }
     
        private string _uriSource;
        private string _uriSource1;
        private bool lightState=false;

        public string UriSource
        {
            get { return _uriSource; }
            set => SetProperty(ref _uriSource, value);
        }

        public string UriSource1
        {
            get { return _uriSource1; }
            set => SetProperty(ref _uriSource1, value);
        }


        public LightControlerPageViewModel(IChartService chartService, ISignalRService signalRService, ILightService lightService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
         
            _chartService = chartService;
            _signalRService = signalRService;
           
            signalRService.InvokeCheckStatusOfLights(true);

            foreach (var light in lightService.StatusModels)
            {
                uriSourceChanger(light.LightStatus,light.ID);
            }
           
            ChangeLightState = new DelegateCommand<string>(async (args) =>
            {
                lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus = !lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus;
                await signalRService.InvokeTurnOnLight(lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus, Int32.Parse(args));
       
                uriSourceChanger(lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus, Int32.Parse(args));
            });
            
            // _wiFiService.ListenHttpRequestsAsync();
         //   _wiFiService.PropertyChanged += _wiFiService_PropertyChanged;
        }

        private async void _wiFiService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      //      uriSourceChanger(_wiFiService.Cmd == "Off");
                  });
        }
        
        private void uriSourceChanger(bool lightState, int lightID)
        {
            switch (lightID)
            {
                case 124:
                    UriSource= lightState ? lightOn : lightOff;
                    break;
                case 125:
                    UriSource1 = lightState ? lightOn : lightOff;
                    break;
            }
        }
    }
}

