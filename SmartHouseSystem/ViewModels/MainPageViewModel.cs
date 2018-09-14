using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SmartHouseSystem.Model;
using SmartHouseSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IChartService chartService;
        private readonly ISignalRService signalRService;
        private readonly ILightService lightService;
        private  double powerOfLightBulbInKiloWats = 0.01;
        private  double usageOffPower;
        public Task Initialization { get; private set; }

        public DelegateCommand SpeechTest { get; private set; }


        public ObservableCollection<StatusModel> statusList;
        public ObservableCollection<PowerUsageModel> powerUsageList;

        public ObservableCollection<StatusModel> StatusList
        {
            get { return statusList; }

            set { SetProperty(ref statusList, value); }
        }

        public ObservableCollection<PowerUsageModel> PowerUsageList
        {
            get { return powerUsageList; }

            set { SetProperty(ref powerUsageList, value); }
        }

        public MainPageViewModel(INavigationService navigationService, ISignalRService signalRService, 
            IChartService chartService, ILightService lightService)
        {
            this.navigationService = navigationService;
            this.chartService = chartService;
            this.lightService = lightService;
            this.signalRService = signalRService;
            // wiFiService.ListenHttpRequestsAsync();

        Initialization = InitAsync();
            //signalRService.InvokeCheckStatusOfLights(true);

            StatusList = new ObservableCollection<StatusModel>
            {
                  new StatusModel("On",this.chartService.BulbOnTimeInMinutes),
                  new StatusModel("Off",this.chartService.BulbOffTimeInMinutes ),
            };
            usageOffPower = (this.chartService.BulbOnTimeInMinutes / 60) * powerOfLightBulbInKiloWats;
            PowerUsageList = new ObservableCollection<PowerUsageModel>
            {
                new PowerUsageModel(usageOffPower,"Wtorek")
            };
            SpeechTest = new DelegateCommand(async () => { await signalRService.ConnectionBuilder( chartService, lightService); });

            Debug.WriteLine("TestMainViewModel");
        
           // this.chartService.PropertyChanged1 += _wiFiService_PropertyChangedAsync;
        }

        private async Task InitAsync()
        {
            await signalRService.InvokeCheckStatusOfLights(true);
        }

        private async void _wiFiService_PropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
          //  await signalRService.InvokeSendStaticticData(chartService.BulbOnTimeInMinutes, chartService.BulbOffTimeInMinutes);
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                   () =>
                       {
                         StatusList.Clear();
                           //Create list of Status list check if
                         StatusList.Add(new StatusModel("On", chartService.Lights[0].BulbOnTimeInMinutes));
                         StatusList.Add(new StatusModel("Off", chartService.Lights[0].BulbOffTimeInMinutes));
                        
                         usageOffPower = (Convert.ToDouble(chartService.BulbOnTimeInMinutes) / 60) * powerOfLightBulbInKiloWats;
                         PowerUsageList.Clear();
                         PowerUsageList.Add(new PowerUsageModel(usageOffPower, "Wtorek")); 
                       });
        }
    }
}
