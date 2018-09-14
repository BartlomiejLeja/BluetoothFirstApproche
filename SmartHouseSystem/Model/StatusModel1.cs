
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHouseSystem.Model
{
    public class StatusModel1 : INotifyPropertyChanged
    {
        private bool _lightStatus;
        private int _ID;
        public StatusModel1(int ID, bool lightStatus)
        {
            _lightStatus = lightStatus;
            _ID = ID;
        }
        
        public bool LightStatus
        { get => _lightStatus;
            set
            {
                _lightStatus = value;
                OnPropertyChanged();
            }
        }

        public int ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
