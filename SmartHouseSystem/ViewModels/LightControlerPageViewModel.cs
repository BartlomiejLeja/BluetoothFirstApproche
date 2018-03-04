using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel: ViewModelBase
    {
        private IWiFiService _wiFiService;
      
        private string lightOn = "ms-appx:///Images/lightTurnOn.png";
        private string lightOff = "ms-appx:///Images/lightTurnOff.jpg";
      
        public ICommand LightOnCommand { get; private set; }
        public DelegateCommand LightOfCommand { get; private set; }
        private string _uriSource;
        private bool lightState =false;
        public string UriSource
        {
            get { return _uriSource; }
            set => SetProperty(ref _uriSource, value); 
        }

        public LightControlerPageViewModel(IWiFiService wiFiService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
            _wiFiService = wiFiService;
      
           LightOnCommand = new DelegateCommand(async () => { await _wiFiService.SendHttpRequestAsync(1);});
           LightOfCommand = new DelegateCommand(async () => {await _wiFiService.SendHttpRequestAsync(0);});
            _wiFiService.ListenHttpRequestsAsync();
            _wiFiService.PropertyChanged += _wiFiService_PropertyChanged;
            
        }

        private void _wiFiService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_wiFiService.Cmd =="On")
                {
                UriSource = lightOn;
                }
            else UriSource = lightOff;
        }

        private void TurnOn()
        {
            Debug.WriteLine("TestViewModelInsideTurnOn");
            _wiFiService.SendHttpRequestAsync(1);
        }


    }
}
