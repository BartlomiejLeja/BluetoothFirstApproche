using System;
using System.Collections.Generic;
using SignalIRServer.Model;

namespace SignalIRServer.Services
{
    public interface ILightsService
    {
        List<LightBulbModel> GetListOfLightBullbs();
        LightBulbModel GetLightModel(int lightBulbID);
        void SetNewBulbStatus(int lightBulbID, bool lightBulbStatus, DateTime dateTime);
    }
}
