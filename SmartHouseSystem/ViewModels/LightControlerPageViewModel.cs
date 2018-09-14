using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System.Diagnostics;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel : ViewModelBase
    {
        private ISignalRService _signalRService;
        private ILightService _lightService;

        private static string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static string lightOff = "ms-appx:///Images/lightTurnOff.jpg";

        public DelegateCommand<string> ChangeLightState { get; private set; }
     
        private string _uriSource;
        private string _uriSource1;
   
        public string UriSource
        {
            get { return _uriSource; }
            set
            {
                SetProperty(ref _uriSource, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string UriSource1
        {
            get { return _uriSource1; }
            set => SetProperty(ref _uriSource1, value);
        }
        public Task Initialization { get; private set; }

        public LightControlerPageViewModel( ISignalRService signalRService, ILightService lightService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
            _signalRService = signalRService;
            _lightService = lightService;
            Initialization = InitAsync();

            foreach (var light in lightService.StatusModels)
            {
                uriSourceChanger(light.LightStatus,light.ID);
            }

            ChangeLightState = new DelegateCommand<string>(async (args) =>
            {
                lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus = !lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus;
                await signalRService.InvokeTurnOnLight(lightService.StatusModels.First(light => light.ID == Int32.Parse(args)).LightStatus, Int32.Parse(args));
            });
            
            lightService.PropertyChanged += LightService_PropertyChanged;
        }

        private async void LightService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var statsuModel in _lightService.StatusModels)
                    {
                        uriSourceChanger(statsuModel.LightStatus, statsuModel.ID);
                    }
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

        private async Task InitAsync()
        {
            await _signalRService.InvokeCheckStatusOfLights(true);
            foreach (var light in _lightService.StatusModels)
            {
                uriSourceChanger(light.LightStatus, light.ID);
            }
        }
    }
}

