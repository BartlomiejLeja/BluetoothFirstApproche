using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;

namespace SmartHouseSystem.Model
{
    public class TimeStatisticsCollectionChartModel : ViewModelBase
    {
       private ObservableCollection<TimeStatisticsChartModel> _timeStatisticsChartModel;
        public string Name { get; set; }
       public ObservableCollection<TimeStatisticsChartModel> TimeStatisticsChartModel
       {
           get => _timeStatisticsChartModel;
           set => SetProperty(ref _timeStatisticsChartModel, value);

        }

        public TimeStatisticsCollectionChartModel(TimeStatisticsChartModel bulbOnStatisticsChartModel, 
            TimeStatisticsChartModel bulbOffStatisticsChartModel, string name)
        {
            Name = name;
            TimeStatisticsChartModel = new ObservableCollection<TimeStatisticsChartModel>
            {
                bulbOnStatisticsChartModel,bulbOffStatisticsChartModel
            };
        }
    }
}
