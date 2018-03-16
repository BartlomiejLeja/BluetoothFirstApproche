namespace SmartHouseSystem.Model
{
    public class StatusModel
    {
        public StatusModel(string name, int time)
        {
            Name = name;
            Time = time;
        }
        public string Name { get; set; }
        public  int Time { get; set; }
    }
}
