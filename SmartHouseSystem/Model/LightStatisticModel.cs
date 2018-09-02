
namespace SmartHouseSystem.Model
{
    public class LightStatisticModel
    {
        public LightStatisticModel(int ID, bool lightStatus)
        {
            this.ID = ID;
            this.LightStatus = lightStatus;
        }
        public int ID;
        private bool lightStatus = false;

        public static string lightOn = "ms-appx:///Images/lightTurnOn.png";
        public static string lightOff = "ms-appx:///Images/lightTurnOff.jpg";

        public int BulbOnTimeInMinutes { get; set; } = 0;
        public int BulbOffTimeInMinutes { get; set; } = 1440;
        public bool LightStatus { get => lightStatus; set => lightStatus = value; }
    }
}
