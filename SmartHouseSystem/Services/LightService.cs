using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartHouseSystem.Model;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartHouseSystem.Services
{
    public class LightService : ILightService
    {
        public ObservableCollection<StatusModel1> StatusModels { get; set; } = new ObservableCollection<StatusModel1>();

        public LightService()
        {
           
        }

        public void InitNotificationOfChange(int lightId)
        {
            StatusModels.First(light => light.ID == lightId).PropertyChanged += Light_PropertyChanged;
        }

        private void Light_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
