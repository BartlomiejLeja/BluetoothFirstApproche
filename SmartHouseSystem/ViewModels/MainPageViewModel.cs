﻿using Prism.Windows.Mvvm;
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
        private readonly ILightService _lightService;
        private ObservableCollection<TimeStatisticsCollectionChartModel> _listOfChartData;
       
        public ObservableCollection<TimeStatisticsCollectionChartModel> ListOfChartData
        {
            get => _listOfChartData;
            set => SetProperty(ref _listOfChartData, value);
        }

        public MainPageViewModel( ILightService lightService)
        {
            _lightService = lightService;
           
            SetStatusListData();

         //   ChartHandler(_lightService);

            lightService.BulbTimePropertyChanged += _bulbTime_PropertyChangedAsync;
        }

        private void SetStatusListData()
        {
            ListOfChartData = new ObservableCollection<TimeStatisticsCollectionChartModel>();
           
            foreach (var lightBulb in _lightService.LightModelList)
            {
                var bulbOnStatisticsChartModel = new TimeStatisticsChartModel("On",
                    lightBulb.BulbOnTimeInMinutesPerDay);
                var bulbOfStatisticsChartModel = new TimeStatisticsChartModel("Off",
                    lightBulb.BulbOffTimeInMinutesPerDay);

                ListOfChartData.Add(new TimeStatisticsCollectionChartModel(bulbOnStatisticsChartModel, bulbOfStatisticsChartModel, lightBulb.Name));
            }
        }
        
        private async void _bulbTime_PropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    ListOfChartData = new ObservableCollection<TimeStatisticsCollectionChartModel>();
                    foreach (var lightBulb in _lightService.LightModelList)
                    {
                        TimeStatisticsChartModel bulbOnStatisticsChartModel;
                        TimeStatisticsChartModel bulbOfStatisticsChartModel;

                        bulbOnStatisticsChartModel = new TimeStatisticsChartModel(
                            "On",
                            lightBulb.BulbOnTimeInMinutesPerDay);
                        bulbOfStatisticsChartModel = new TimeStatisticsChartModel(
                            "Off",
                            lightBulb.BulbOffTimeInMinutesPerDay);

                        ListOfChartData.Add(new TimeStatisticsCollectionChartModel(bulbOnStatisticsChartModel, bulbOfStatisticsChartModel, lightBulb.Name));
                    }
                });
        }

    }
}

