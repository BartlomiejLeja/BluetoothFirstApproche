namespace SignalIRServer.Model
{
    public class LightModel
    {
        public LightModel(int ID, bool lightStatus, string name)
        {
            this.LightStatus = lightStatus;
            this.ID = ID;
            this.Name = name;
        }

        public bool LightStatus { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
