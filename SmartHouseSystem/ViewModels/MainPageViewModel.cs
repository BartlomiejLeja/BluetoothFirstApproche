using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Model;
using SmartHouseSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Core;

namespace SmartHouseSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IChartService chartService;
        private readonly ILightService lightService;
        private  double powerOfLightBulbInKiloWats = 0.01;
        private  double usageOffPower;
     
        public DelegateCommand SpeechTest { get; private set; }
        
        public ObservableCollection<TimeStatisticsChartModel> statusList;
        public ObservableCollection<PowerUsageModel> powerUsageList;

        public ObservableCollection<TimeStatisticsChartModel> StatusList
        {
            get { return statusList; }

            set { SetProperty(ref statusList, value); }
        }

        public ObservableCollection<PowerUsageModel> PowerUsageList
        {
            get { return powerUsageList; }

            set { SetProperty(ref powerUsageList, value); }
        }

        public MainPageViewModel(IChartService chartService, ILightService lightService)
        {
            this.chartService = chartService;
            this.lightService = lightService;
         
//            usageOffPower = (this.chartService.BulbOnTimeInMinutes / 60) * powerOfLightBulbInKiloWats;
//            PowerUsageList = new ObservableCollection<PowerUsageModel>
//            {
//                new PowerUsageModel(usageOffPower,"Wtorek")
//            };
            SetStatusListData();
            SpeechTest = new DelegateCommand(async () => { StatusList = lightService.LightModelList[0].TimeStatisticsChartModelObservableCollection; });
            lightService.BulbTimePropertyChanged += _bulbTime_PropertyChangedAsync;
        }

        private void SetStatusListData()
        {
            StatusList = new ObservableCollection<TimeStatisticsChartModel>
            {
                //Create list of Status list check if
                new TimeStatisticsChartModel("On", lightService.LightModelList[0].BulbOnTimeInMinutesPerDay),
                new TimeStatisticsChartModel("Off", lightService.LightModelList[0].BulbOffTimeInMinutesPerDay)
            };
        }

        private async void _bulbTime_PropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                   SetStatusListData);
        }
    }
}
