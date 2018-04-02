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
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
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
        private DateTime OnLightTime;
        private DateTime OffLightTime;
        private bool endOfCounter= false;

        private bool timerTriger;
        public bool TimerTriger { get => timerTriger; set { timerTriger = value; TimerTrigerNotifyPropertyChanged(nameof(timerTriger)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void TimerTrigerNotifyPropertyChanged(String propertyName) =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand SignalRConnectionCommand { get; private set; }
       
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
            this.globalDataStorageService = globalDataStorageService;
            var tempOn = wiFiService.LightOnDataTime;
            var tempOff = wiFiService.LightOffDataTime;
            globalDataStorageService.LightOnInMinutes(tempOn, tempOff);
            StatusList = new ObservableCollection<StatusModel>
            {
                  new StatusModel("On",this.globalDataStorageService.BulbOnTimeInMinutes),
                  new StatusModel("Off",this.globalDataStorageService.BulbOffTimeInMinutes ),
            };
            Debug.WriteLine("TestMainViewModel");
            this.navigationService = navigationService;
            this.wiFiService = wiFiService;
         
            OnLightTime = wiFiService.LightOnDataTime;
            OffLightTime = wiFiService.LightOffDataTime;
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
         //   wiFiService.PropertyChanged1 += WiFiService_PropertyChanged1;
            PropertyChanged += _wiFiService_PropertyChanged;
            TimerTriger = true;
            signalRService.Connect(wiFiService);
        }

        private void WiFiService_PropertyChanged1(object sender, PropertyChangedEventArgs e)
        {
            var tempOn = wiFiService.LightOnDataTime;
            var tempOff =  wiFiService.LightOffDataTime;
            globalDataStorageService.LightOnInMinutes(tempOn, tempOff);
        }

        private void _wiFiService_PropertyChanged(object sender, PropertyChangedEventArgs e) //odjałem async
        {
                TimeSpan delay = TimeSpan.FromSeconds(20);

                ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreatePeriodicTimer(
                         async (source) =>
                           {
                               if (wiFiService.Cmd == "On" && endOfCounter == false)
                               {
                                   globalDataStorageService.BulbOffTimeInMinutes--;
                                   globalDataStorageService.BulbOnTimeInMinutes++;
                                   await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                    () =>
                                         {
                                             StatusList.Clear();

                                             StatusList.Add(new StatusModel("On", globalDataStorageService.BulbOnTimeInMinutes));
                                             StatusList.Add(new StatusModel("Off", globalDataStorageService.BulbOffTimeInMinutes));
                                         });
                               }
                           }, delay);
            if (wiFiService.Cmd == "Off" || endOfCounter == true || wiFiService.Cmd == null )
            { DelayTimer.Cancel(); };
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
            endOfCounter = true;
            navigationService.Navigate("CameraViewer", null);
        }
        private void LightControlerPage()
        {
            endOfCounter = true;
            navigationService.Navigate("LightControler", null);
        }
    }
}
