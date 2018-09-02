using System.Collections.Generic;
using System.ComponentModel;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface IChartService
    {
        int BulbOnTimeInMinutes { get; set; }
        int BulbOffTimeInMinutes { get; set; }
      

        void ChartHandler(bool lightStatus, int lightNumber);
        List<LightStatisticModel> Lights { get; set; }
        bool IsTimerOn { get; set; }
        event PropertyChangedEventHandler PropertyChanged1;
        event PropertyChangedEventHandler PropertyChanged;
    }
}
