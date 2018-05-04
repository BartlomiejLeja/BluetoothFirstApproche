﻿using Prism.Commands;
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
        private double powerOfLightBulbInKiloWats = 0.01;
        private double usageOffPower;

        public ICommand SignalRConnectionCommand { get; private set; }
       
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
            usageOffPower = (this.chartService.BulbOnTimeInMinutes / 60) * powerOfLightBulbInKiloWats;
            PowerUsageList = new ObservableCollection<PowerUsageModel>
            {
                new PowerUsageModel(usageOffPower,"Wtorek")
            };
            Debug.WriteLine("TestMainViewModel");
           
            SignalRConnectionCommand = new DelegateCommand(() => signalRService.InvokeSendMethod("Lol"));
            
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
                         usageOffPower = (Convert.ToDouble(chartService.BulbOnTimeInMinutes) / 60) * powerOfLightBulbInKiloWats;
                         PowerUsageList.Clear();
                         PowerUsageList.Add(new PowerUsageModel(usageOffPower, "Wtorek")); 
                       });
        }
    }
}
