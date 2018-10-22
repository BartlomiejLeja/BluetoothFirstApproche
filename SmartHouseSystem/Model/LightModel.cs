using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHouseSystem.Model
{
    public class LightModel 
    {
        private bool _lightStatus;
        private int _ID;
        private double _bulbOnTimeInMinutesPerDay = 1440;
        private string _name;
        public DateTime TimeOn { get; set; }
        public DateTime TimeOff { get; set; }

        public LightModel(int ID, bool lightStatus, string name)
        {
            _lightStatus = lightStatus;
            _name = name;
            _ID = ID;
            TimeStatisticsChartModelObservableCollection = new ObservableCollection<TimeStatisticsChartModel>()
            {
                new TimeStatisticsChartModel("On", BulbOnTimeInMinutesPerDay),
                new TimeStatisticsChartModel("Off",BulbOffTimeInMinutesPerDay),
            };
        }
        
        public bool LightStatus
        { get => _lightStatus;
            set
            {
                _lightStatus = value;
                OnStatusOrIdLightModelPropertyChanged();
            }
        }

        public string Name { get => _name; set=>_name=value; }

        public int ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnStatusOrIdLightModelPropertyChanged();
            }
        }

        public double BulbOnTimeInMinutesPerDay
        {
            get => _bulbOnTimeInMinutesPerDay;
            set
            {
                _bulbOnTimeInMinutesPerDay = value;
                OnBulbTimePropertyChanged();
            }
        }
        public double BulbOffTimeInMinutesPerDay { get; set; } = 0;

        public ObservableCollection<TimeStatisticsChartModel> TimeStatisticsChartModelObservableCollection { get; set; }

        public event PropertyChangedEventHandler StatusOrIdLightModelPropertyChanged;

        protected virtual void OnStatusOrIdLightModelPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StatusOrIdLightModelPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler BulbTimePropertyChanged;

        protected virtual void OnBulbTimePropertyChanged([CallerMemberName] string propertyName = null)
        {
            BulbTimePropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
