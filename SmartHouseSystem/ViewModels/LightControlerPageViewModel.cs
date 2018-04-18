using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System.Diagnostics;
using System.Windows.Input;
using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel : ViewModelBase
    {
        private IWiFiService _wiFiService;
     
        private static string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static string lightOff = "ms-appx:///Images/lightTurnOff.jpg";

        public ICommand ChangeLightState { get; private set; }
        public DelegateCommand LightOfCommand { get; private set; }
        private string _uriSource;
        private bool lightState;

        public string UriSource
        {
            get { return _uriSource; }
            set => SetProperty(ref _uriSource, value);
        }

        public LightControlerPageViewModel(IWiFiService wiFiService,  IChartService chartService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
         
            _wiFiService = wiFiService;
            
            var pinStatusJason = _wiFiService.CheckStatusOfLight().GetAwaiter().GetResult();
         
            lightState = Convert.ToBoolean(JsonConvert.DeserializeObject<LightModel>(pinStatusJason).state);
     
            uriSourceChanger(lightState);

            ChangeLightState = new DelegateCommand(async () => { await _wiFiService.SendHttpRequestAsync(lightState);
                lightState = !lightState;
                uriSourceChanger(lightState);
                chartService.IsTimerOn = !lightState;
            });

           // _wiFiService.ListenHttpRequestsAsync();
            _wiFiService.PropertyChanged += _wiFiService_PropertyChanged;
        }

        private void _wiFiService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            uriSourceChanger(_wiFiService.Cmd == "Off");
        }

        private void uriSourceChanger(bool lightState)
        {
            UriSource= lightState ? lightOff : lightOn;
        }
    }
}
