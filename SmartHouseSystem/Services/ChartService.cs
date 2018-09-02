using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;
using SmartHouseSystem.Model;
using System.Linq;

namespace SmartHouseSystem.Services
{
    public class ChartService :IChartService, INotifyPropertyChanged
    {
        private int bulbOnTimeInMinutes = 0;
        private int bulbOffTimeInMinutes = 1440;
        bool isTimerOn = false;
        public int BulbOnTimeInMinutes { get => bulbOnTimeInMinutes; set { bulbOnTimeInMinutes = value; BulbOnTimeTrigerNotifyPropertyChanged(nameof(bulbOnTimeInMinutes)); } }
        public int BulbOffTimeInMinutes { get => bulbOffTimeInMinutes; set => bulbOffTimeInMinutes = value;}
        public bool IsTimerOn { get => isTimerOn; set { isTimerOn = value; TimerTrigerNotifyPropertyChanged(nameof(isTimerOn)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged1;
        private DateTime firstDate;


        private List<LightStatisticModel> lights = new List<LightStatisticModel>();
        public List<LightStatisticModel> Lights
        {
            get => lights;
            set => lights = value;
        }

        internal void BulbOnTimeTrigerNotifyPropertyChanged(String propertyName) =>
        PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        internal void TimerTrigerNotifyPropertyChanged(String propertyName) =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ChartService()
        {
           // PropertyChanged += ChartService_PropertyChanged;
            firstDate = DateTime.Now;
        }
        
        private void ChartService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TimeSpan delay = TimeSpan.FromSeconds(20);

            ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreatePeriodicTimer(
                     (source) =>
                     {
                         if (isTimerOn == true )
                         {
                             if (firstDate.Day == DateTime.Now.Day)
                             {
                                 //HOW TO OBSERVE PROPERTIES IN OBSERVABLE LIST !!!!!???
                                 BulbOffTimeInMinutes--;
                                 BulbOnTimeInMinutes++;
                                 Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {BulbOffTimeInMinutes} and bulbOffTimeInMinutes changed, new value {BulbOnTimeInMinutes} date: {DateTime.Now}");
                             }
                             else
                             {
                                 BulbOffTimeInMinutes = 1439;
                                 BulbOnTimeInMinutes = 1;
                                 firstDate = DateTime.Now;
                             }
                         }

                     }, delay);
            if (isTimerOn == false)
            { DelayTimer.Cancel(); };
        }

        public void ChartHandler(bool lightStatus, int lightNumber)
        {
            if (lights.Any(light => light.ID != lightNumber) || lights.Count == 0)
            {
                lights.Add(new LightStatisticModel(lightNumber, lightStatus));
            }
            else lights.First(light => light.ID == lightNumber).LightStatus = lightStatus;


            var delay = TimeSpan.FromSeconds(30);

            var DelayTimer = ThreadPoolTimer.CreatePeriodicTimer(
                (source) =>
                {
                        if (firstDate.Day == DateTime.Now.Day)
                        {
                            //event triger in MainPageViewModel
                            BulbOnTimeInMinutes++;

                            foreach(var light in Lights)
                            {
                                if (light.LightStatus)
                                {
                                    light.BulbOnTimeInMinutes--;
                                    light.BulbOffTimeInMinutes++;
                                    Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {light.BulbOffTimeInMinutes} " +
                                                    $"and bulbOffTimeInMinutes changed, new value {light.BulbOnTimeInMinutes} date: {DateTime.Now}");
                                }
                            }
                            //HOW TO OBSERVE PROPERTIES IN OBSERVABLE LIST !!!!!???
                            //foreach lights list check if status is on ++ on -- off
                          //  lights.First(light => light.ID == lightNumber).BulbOffTimeInMinutes--;
                            //BulbOffTimeInMinutes--;
                          
                          //  lights.First(light => light.ID == lightNumber).BulbOnTimeInMinutes++;
//                            Debug.WriteLine($"Value of bulbOffTimeInMinutes changed, new value {lights.First(light => light.ID == lightNumber).BulbOffTimeInMinutes} " +
//                                            $"and bulbOffTimeInMinutes changed, new value {lights.First(light => light.ID == lightNumber).BulbOnTimeInMinutes} date: {DateTime.Now}");
                        }
                        else
                        {
                            lights.First(light => light.ID == lightNumber).BulbOffTimeInMinutes = 1439;
                            lights.First(light => light.ID == lightNumber).BulbOnTimeInMinutes = 1;
                            firstDate = DateTime.Now;
                        } 
                }, delay);
//            if (lightStatus == false)
//            { DelayTimer.Cancel(); };
        }
    }
}
