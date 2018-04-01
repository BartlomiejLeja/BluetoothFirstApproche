using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SmartHouseSystem.Model;
using SmartHouseSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SmartHouseSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool isPaneOpen =false;
        private readonly INavigationService navigationService;
        private readonly IWiFiService wiFiService;
        private readonly IGlobalDataStorageService globalDataStorageService;
        public ICommand OpenHamburgerMenuCommand { get; private set; }
        public ICommand CameraViewerPageCommand { get; private set; }
        public ICommand ESPViewerPageCommand { get; private set; }
        private int bulbOnCounter=0;
        private int bulbOffCounter =1440;
        
        public ICommand SignalRConnectionCommand { get; private set; }
        Random rand = new Random();
        public ObservableCollection<StatusModel> statusList;

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }

            set { SetProperty(ref isPaneOpen, value); }
        }

        public ObservableCollection<StatusModel> StatusList
        {
            get { return statusList; }

            set { SetProperty(ref statusList, value); }
        }

        public MainPageViewModel(INavigationService navigationService, ISignalRService signalRService, 
            IWiFiService wiFiService, IGlobalDataStorageService globalDataStorageService)
        {
            StatusList = new ObservableCollection<StatusModel>
            {
                  new StatusModel("On",bulbOnCounter),
                  new StatusModel("Off",bulbOffCounter ),
            };
            Debug.WriteLine("TestMainViewModel");
            this.navigationService = navigationService;
            this.wiFiService = wiFiService;
            this.globalDataStorageService = globalDataStorageService;
            //OpenHamburgerMenuCommand = new DelegateCommand(async () =>
            //{
            //    IsPaneOpen = !isPaneOpen;
            //});
            OpenHamburgerMenuCommand = new DelegateCommand(HamburgerMenuButton);
            CameraViewerPageCommand = new DelegateCommand(CameraViewerPage);
            ESPViewerPageCommand = new DelegateCommand(LightControlerPage);
            SignalRConnectionCommand = new DelegateCommand(() => counterMenager()/*signalRService.InvokeSendMethod()*/);
            wiFiService.ListenHttpRequestsAsync();
            wiFiService.PropertyChanged += _wiFiService_PropertyChanged;
            signalRService.Connect(wiFiService);
        }

        private async void _wiFiService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
                TimeSpan delay = TimeSpan.FromSeconds(15);

                ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreatePeriodicTimer(
                    (source) =>
                    {
                        if (wiFiService.Cmd == "On")
                        {
                            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                      () =>
                      {

                          StatusList.Clear();
                          bulbOffCounter--;
                          bulbOnCounter++;
                          globalDataStorageService.BulbOnTimeInMinutes = bulbOnCounter;
                          StatusList.Add(new StatusModel("On", bulbOnCounter));
                          StatusList.Add(new StatusModel("Of", bulbOffCounter));

                      });
                        }
                    }, delay );
              
            if (wiFiService.Cmd == "Off") DelayTimer.Cancel();
        }

      
        //HOW TO OBSERVE PROPERTIES IN OBSERVABLE LIST !!!!!???
        private void counterMenager()
        {
            //statusList[1].Time--;
            //statusList[0].Time++;
            StatusList.Clear();
            bulbOffCounter --;
            bulbOnCounter++;
            StatusList.Add(new StatusModel("On", bulbOnCounter));
            StatusList.Add(new StatusModel("Of", bulbOffCounter ));
        }
        private void HamburgerMenuButton()
        {
            IsPaneOpen = !isPaneOpen;
        }

        private void CameraViewerPage()
        {
            navigationService.Navigate("CameraViewer", null);
        }
        private void LightControlerPage()
        {
            navigationService.Navigate("LightControler", null);
        }
    }
}
