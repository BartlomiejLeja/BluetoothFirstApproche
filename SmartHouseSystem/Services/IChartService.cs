using System.ComponentModel;

namespace SmartHouseSystem.Services
{
    public interface IChartService
    {
        int BulbOnTimeInMinutes { get; set; }
        int BulbOffTimeInMinutes { get; set; }
        bool IsTimerOn { get; set; }
        event PropertyChangedEventHandler PropertyChanged1;
    }
}
