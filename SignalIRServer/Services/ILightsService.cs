using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalIRServer.Model;

namespace SignalIRServer.Services
{
    public interface ILightsService
    {
        List<LightModel> GetListOfLightBullbs();
        void SetNewBulbStatus(int lightBulbID, bool lightBulbStatus);
    }
}
