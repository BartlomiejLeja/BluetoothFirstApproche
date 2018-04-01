using System;

namespace SmartHouseSystem.Model
{
    public class StatusModel
    {
        private DateTime _dateTime;
        public StatusModel(string name, int time)
        {
            Name = name;
            Time = time;
        }
      
        public string Name { get; set; }
        public int Time { get; set; }
     
        public DateTime DataTime { get=>_dateTime; set => _dateTime = DateTime.Now; }

    }
}
