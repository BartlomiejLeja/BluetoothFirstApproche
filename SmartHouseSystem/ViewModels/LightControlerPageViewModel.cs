using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel: ViewModelBase
    {
        private IWiFiService _wiFiService;
      
        private static string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static string lightOff = "ms-appx:///Images/lightTurnOff.jpg";
      
        public ICommand LightOnCommand { get; private set; }
        public DelegateCommand LightOfCommand { get; private set; }
        private string _uriSource= lightOff;
        private bool lightState =true;
        public string UriSource
        {
            get { return _uriSource; }
            set => SetProperty(ref _uriSource, value); 
        }

        public LightControlerPageViewModel(IWiFiService wiFiService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
            _wiFiService = wiFiService;
      
           LightOnCommand = new DelegateCommand(async () => { await _wiFiService.SendHttpRequestAsync(lightState);
               lightState = !lightState; if(lightState) UriSource = lightOn; else UriSource = lightOff; });
       
            _wiFiService.ListenHttpRequestsAsync();
            _wiFiService.PropertyChanged += _wiFiService_PropertyChanged;
            
        }

        private void _wiFiService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_wiFiService.Cmd =="On") UriSource = lightOn;   
            else UriSource = lightOff;
        }

        private void TurnOn()
        {
            Debug.WriteLine("TestViewModelInsideTurnOn");
         //   _wiFiService.SendHttpRequestAsync(1);
        }


    }
}
