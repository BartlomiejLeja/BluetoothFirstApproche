using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel : ViewModelBase
    {
        private readonly ILightService _lightService;

        private static readonly string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static  string lightOff = "ms-appx:///Images/lightTurnOff.jpg";

        public DelegateCommand<string> ChangeLightState { get; private set; }
     
        private string _uriSource;
        private string _uriSource1;
   
        public string UriSource
        {
            get => _uriSource;
            set => SetProperty(ref _uriSource, value);
        }
        
        public string UriSource1
        {
            get => _uriSource1;
            set => SetProperty(ref _uriSource1, value);
        }

        public LightControlerPageViewModel( ISignalRService signalRService, ILightService lightService)
        {
            _lightService = lightService;
     
            foreach (var light in lightService.LightModelList)
            {
                UriSourceChanger(light.LightStatus,light.ID);
            }

            ChangeLightState = new DelegateCommand<string>(async (args) =>
            {
                lightService.LightModelList.First(light => light.ID == Int32.Parse(args)).LightStatus = !lightService.LightModelList.First(light => light.ID == Int32.Parse(args)).LightStatus;
                await signalRService.InvokeTurnOnLight(lightService.LightModelList.First(light => light.ID == Int32.Parse(args)).LightStatus, Int32.Parse(args));
            });
            
            lightService.StatusOfLightPropertyChanged += LightService_PropertyChanged;
        }

        private async void LightService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var statsuModel in _lightService.LightModelList)
                    {
                        UriSourceChanger(statsuModel.LightStatus, statsuModel.ID);
                    }
                });
        }
       
        private void UriSourceChanger(bool lightState, int lightID)
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

