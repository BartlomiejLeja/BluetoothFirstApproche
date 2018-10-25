using System;

namespace SignalIRServer.Model
{
    public class LightBulbModel
    {
        public LightBulbModel()
        {

        }
        public LightBulbModel(int ID, bool lightStatus, string name)
        {
            this.LightStatus = lightStatus;
            this.ID = ID;
            this.Name = name;
        }

        public bool LightStatus { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime TimeOn { get; set; }
        public DateTime TimeOff { get; set; }
        public double BulbOnTimeInMinutesPerDay { get; set; } = 0;
        public double BulbOffTimeInMinutesPerDay { get; set; } = 1440;
    }
}
