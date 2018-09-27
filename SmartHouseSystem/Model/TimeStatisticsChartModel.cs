namespace SmartHouseSystem.Model
{
    public class TimeStatisticsChartModel
    {
        public TimeStatisticsChartModel(string name, int time)
        {
            Name = name;
            Time = time;
        }
      
        public string Name { get; set; }
        public int Time { get; set; }
    }
}
