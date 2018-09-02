namespace SignalIRServer.Model
{
    public class StatusModel
    {
        public StatusModel(int ID, bool lightStatus)
        {
            this.lightStatus = lightStatus;
            _ID = ID;
        }

        private bool lightStatus;
        private int _ID;

        public bool LightStatus { get => lightStatus; set => lightStatus = value; }
        public int ID { get => _ID; set => _ID = value; }
    }
}

