using Prism.Windows.Mvvm;

namespace SmartHouseSystem.Model
{
    public class TimeStatisticsChartModel : ViewModelBase
    {
        private int _time;
        public TimeStatisticsChartModel(string name, int time)
        {
            Name = name;
            Time = time;
        }
      
        public string Name { get; set; }

        public int Time
        {
            get=> _time;
            set=>SetProperty(ref _time,value);
        }
    }
}
