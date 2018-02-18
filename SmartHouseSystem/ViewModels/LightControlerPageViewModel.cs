using Prism.Commands;
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
    public class LightControlerPageViewModel
    {
        private IWiFiService _wiFiService;
        public ICommand LightOnCommand { get; private set; }
        public DelegateCommand LightOfCommand { get; private set; }

        public LightControlerPageViewModel(IWiFiService wiFiService)
        {
            Debug.WriteLine("TestViewModelInsideConstructor");
            _wiFiService = wiFiService;

           LightOnCommand = new DelegateCommand(async () => {await _wiFiService.SendHttpRequestAsync(1); });
           LightOfCommand = new DelegateCommand(async () => {await _wiFiService.SendHttpRequestAsync(0); });
        }

        private void TurnOn()
        {
            Debug.WriteLine("TestViewModelInsideTurnOn");
            _wiFiService.SendHttpRequestAsync(1);
        }
    }
}
