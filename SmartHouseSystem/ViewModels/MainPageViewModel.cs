using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SmartHouseSystem.Model;
using SmartHouseSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IWiFiService wiFiService;
        private readonly IChartService chartService;
        
        public ICommand SignalRConnectionCommand { get; private set; }
       
        public ObservableCollection<StatusModel> statusList;
        
        public ObservableCollection<StatusModel> StatusList
        {
            get { return statusList; }

            set { SetProperty(ref statusList, value); }
        }

        public MainPageViewModel(INavigationService navigationService, ISignalRService signalRService, 
            IWiFiService wiFiService,  IChartService chartService)
        {
            this.navigationService = navigationService;
            this.wiFiService = wiFiService;
            this.chartService = chartService;
            wiFiService.ListenHttpRequestsAsync();
            signalRService.Connect(wiFiService);

            StatusList = new ObservableCollection<StatusModel>
            {
                  new StatusModel("On",this.chartService.BulbOnTimeInMinutes),
                  new StatusModel("Off",this.chartService.BulbOffTimeInMinutes ),
            };
            Debug.WriteLine("TestMainViewModel");
           
            SignalRConnectionCommand = new DelegateCommand(() => signalRService.InvokeSendMethod());
            
            this.chartService.PropertyChanged1 += _wiFiService_PropertyChangedAsync;
        }

        private async void _wiFiService_PropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                   () =>
                       {
                         StatusList.Clear();
                         StatusList.Add(new StatusModel("On", chartService.BulbOnTimeInMinutes));
                         StatusList.Add(new StatusModel("Off", chartService.BulbOffTimeInMinutes));
                       });
        }
    }
}
