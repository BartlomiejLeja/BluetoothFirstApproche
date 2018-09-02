namespace SmartHouseSystem.Model
{
   public class PowerUsageModel
    {
        public PowerUsageModel(double kilowattHour, string day)
        {
            Day = day;
            KilowattHour = kilowattHour;
        }

        public double KilowattHour { get; set; }
        public string Day { get; set; }
    }
}
