
using System;

namespace SmartHouseSystem.Services
{
    public interface IGlobalDataStorageService
    {
        int BulbOnTimeInMinutes { get; set; }
        int BulbOffTimeInMinutes { get; set; }
        void LightOnInMinutes(DateTime dateTimeOn, DateTime dateTimeOff);
    }
}
