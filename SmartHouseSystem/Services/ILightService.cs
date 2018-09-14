using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface ILightService
    {
        ObservableCollection<StatusModel1> StatusModels { get; set; }

        void InitNotificationOfChange(int lightId);

        event PropertyChangedEventHandler PropertyChanged;
    }
}
