using System;

namespace SmartHouseSystem.Services
{
    public class GlobalDataStorageService : IGlobalDataStorageService
    {
        private int bulbOnTimeInMinutes = 0;
        private int bulbOffTimeInMinutes = 1440;
        private DateTime nullDataTime =new DateTime(1,1,0001,12,00,00);
        private DateTime oldOnDataTime;
        private DateTime oldOffDataTime;
        private DateTime oneDayCounterRule;

        public int BulbOnTimeInMinutes { get => bulbOnTimeInMinutes; set => bulbOnTimeInMinutes = value; }
        public int BulbOffTimeInMinutes { get => bulbOffTimeInMinutes; set => bulbOffTimeInMinutes = value; }

        public GlobalDataStorageService()
        {
            oneDayCounterRule = DateTime.Now;
        }
        public void LightOnInMinutes(DateTime dateTimeOn , DateTime dateTimeOff)
        {
            //if (oldOnDataTime == dateTimeOn && oldOffDataTime == dateTimeOff)
            //    return;
         
           if (dateTimeOff.Year != nullDataTime.Year && dateTimeOn.Year != nullDataTime.Year)
            {
             BulbOffTimeInMinutes = (dateTimeOff - dateTimeOn).Minutes;
             BulbOnTimeInMinutes = (dateTimeOff - dateTimeOn).Minutes;
            }

            //if (dateTimeOff.Year == nullDataTime.Year && dateTimeOn.Year == nullDataTime.Year)
            //     return 1;
            if (dateTimeOn.Year != nullDataTime.Year && dateTimeOff.Year == nullDataTime.Year)
            {
                BulbOffTimeInMinutes -= (DateTime.Now - dateTimeOn).Minutes;
                BulbOnTimeInMinutes += (DateTime.Now - dateTimeOn).Minutes;
            }
            //TODO what if someone turnOn light for whole day 
            if(oneDayCounterRule.Date.ToString("d")!=DateTime.Now.ToString("d"))
            {
                oneDayCounterRule.AddDays(1);
                bulbOnTimeInMinutes = 0;
                bulbOffTimeInMinutes = 1440;
             }
            oldOnDataTime = dateTimeOn;
            oldOffDataTime = dateTimeOff;
        }

       
    }
}
