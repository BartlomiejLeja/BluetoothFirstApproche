using System;
using System.ComponentModel;
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;

namespace SmartHouseSystem.Services
{
    public class ChartService :IChartService, INotifyPropertyChanged
    {
        private int bulbOnTimeInMinutes = 0;
        private int bulbOffTimeInMinutes = 1440;
        bool isTimerOn = false;
        public int BulbOnTimeInMinutes { get => bulbOnTimeInMinutes; set { bulbOnTimeInMinutes = value; BulbOnTimeTrigerNotifyPropertyChanged(nameof(bulbOnTimeInMinutes)); } }
        public int BulbOffTimeInMinutes { get => bulbOffTimeInMinutes; set { bulbOffTimeInMinutes = value; BulbOnTimeTrigerNotifyPropertyChanged(nameof(bulbOnTimeInMinutes)); } }
        public bool IsTimerOn { get => isTimerOn; set { isTimerOn = value; TimerTrigerNotifyPropertyChanged(nameof(isTimerOn)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged1;

        internal void BulbOnTimeTrigerNotifyPropertyChanged(String propertyName) =>
        PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        internal void TimerTrigerNotifyPropertyChanged(String propertyName) =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ChartService()
        {
            PropertyChanged += ChartService_PropertyChanged;   
        }
        
        private void ChartService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TimeSpan delay = TimeSpan.FromSeconds(20);

            ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreatePeriodicTimer(
                     (source) =>
                     {
                         if (isTimerOn == true)
                         {
                             //HOW TO OBSERVE PROPERTIES IN OBSERVABLE LIST !!!!!???
                             BulbOffTimeInMinutes--;
                             BulbOnTimeInMinutes++;
                             Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {BulbOffTimeInMinutes} and bulbOffTimeInMinutes changed, new value {BulbOnTimeInMinutes} date: {DateTime.Now}");
                         }
                     }, delay);
            if (isTimerOn == false)
            { DelayTimer.Cancel(); };
        }
    }
}
