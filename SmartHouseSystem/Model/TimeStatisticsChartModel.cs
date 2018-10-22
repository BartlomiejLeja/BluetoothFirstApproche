using Prism.Windows.Mvvm;

namespace SmartHouseSystem.Model
{
    public class TimeStatisticsChartModel : ViewModelBase
    {
        private double _time;
        public TimeStatisticsChartModel(string name, double time)
        {
            Name = name;
            Time = time;
        }
      
        public string Name { get; set; }

        public double Time
        {
            get=> _time;
            set=>SetProperty(ref _time,value);
        }
    }
}
