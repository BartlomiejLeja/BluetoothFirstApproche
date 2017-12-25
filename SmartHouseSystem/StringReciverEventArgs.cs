using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseSystem
{
    class StringReciverEventArgs : INotifyPropertyChanged
    {
        private string message = string.Empty;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("MessageFromArduino");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string message)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
        }
    }
}
