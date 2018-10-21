using System.Collections.Generic;
using System.Linq;
using SignalIRServer.Model;

namespace SignalIRServer.Services
{
    public class LightsService : ILightsService
    {
        private readonly List<LightModel> _lightBullbModelList = new List<LightModel>();

        public LightsService()
        {
            _lightBullbModelList.Add(new LightModel(1, false, "First"));
            _lightBullbModelList.Add(new LightModel(2, false, "Second"));
        }
        
        public List<LightModel> GetListOfLightBullbs()
        {
            return _lightBullbModelList;
        }

        public void SetNewBulbStatus(int lightBulbID, bool lightBulbStatus)
        {
            _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).LightStatus = lightBulbStatus;
        }
    }
}
