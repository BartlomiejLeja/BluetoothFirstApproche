using System.Windows.Input;
using Prism.Windows.Mvvm;

namespace SmartHouseSystem.Model
{
    public class DisplayLightModel : ViewModelBase
    {
        private string _lightStatus;
        public string LightStatus { get => _lightStatus;
            set=>SetProperty(ref _lightStatus,value);
        }
        public string Name { get; set; }
        public string LightBulbNumber { get; set; }
        public ICommand Command { get; set; }
    }
}
