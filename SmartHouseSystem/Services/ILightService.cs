using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface ILightService
    {
        ObservableCollection<LightModel> LightModelList { get; set; }
     
        void InitNotificationOfChange(int lightId);
        void InitNotificationOfBulbChange(LightModel lightModel);

        event PropertyChangedEventHandler StatusOfLightPropertyChanged;
        event PropertyChangedEventHandler BulbTimePropertyChanged;

    }
}
