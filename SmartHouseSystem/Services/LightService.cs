using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartHouseSystem.Model;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartHouseSystem.Services
{
    public class LightService : ILightService
    {
        public ObservableCollection<LightModel> LightModelList { get; set; } = new ObservableCollection<LightModel>();

        public LightService()
        {
           
        }

        public void InitNotificationOfChange(int lightId)
        {
            LightModelList.First(light => light.ID == lightId).StatusOrIdLightModelPropertyChanged += LightStatusOrIdLightModelStatusOrIdLightModelPropertyChanged;
            LightModelList.First(light => light.ID == lightId).BulbTimePropertyChanged += LightService_BulbTimePropertyChanged;
        }

        private void LightService_BulbTimePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnBulbTimePropertyChanged();
        }

        private void LightStatusOrIdLightModelStatusOrIdLightModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnStatusOfLightPropertyChanged();
        }
        
        public event PropertyChangedEventHandler StatusOfLightPropertyChanged;

        protected virtual void OnStatusOfLightPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StatusOfLightPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler BulbTimePropertyChanged;

        protected virtual void OnBulbTimePropertyChanged([CallerMemberName] string propertyName = null)
        {
            BulbTimePropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
