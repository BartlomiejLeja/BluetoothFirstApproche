using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SignalIRServer.Model;

namespace SignalIRServer.Services
{
    public class LightsService : ILightsService
    {
        private readonly List<LightModel> _lightBullbModelList = new List<LightModel>();
        private DateTime _compareValue= new DateTime(0001, 1, 1);
        private readonly ILogger _logger;

        public LightsService(ILogger<LightsService> logger)
        {
            _logger = logger;
            _lightBullbModelList.Add(new LightModel(1, false, "First"));
            _lightBullbModelList.Add(new LightModel(2, false, "Second"));
        }

        public void SetTime(int lightBulbID, bool lightBulbStatus, DateTime dateTime)
        {
            if (lightBulbStatus == true)

            {
                _logger.LogInformation($"IS IN lightBulbStatus True  {dateTime}");
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn = dateTime;
            }

            if (lightBulbStatus == false)
            {
                _logger.LogInformation($"IS IN lightBulbStatus False Date off {dateTime}");
                _logger.LogInformation($"IS IN lightBulbStatus False Date on {_lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn}");
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOff = dateTime;
                double resultMilliseconds = (_lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOff
                                      - _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn)
                    .Milliseconds;
                _logger.LogInformation($"ResultInmiliseconds {resultMilliseconds}");
                double resultMinures1 = (_lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOff
                                             - _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn)
                    .TotalMinutes;
             

                double resultInMinutes =resultMilliseconds / 60000;

                _logger.LogInformation($"ResultInMinutes {resultInMinutes}");
                _logger.LogInformation($"ResultInMinutes1 {resultMinures1}");
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).BulbOnTimeInMinutesPerDay +=
                    resultMinures1;
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).BulbOffTimeInMinutesPerDay -=
                    resultMinures1;

                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn = new DateTime();
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOff = new DateTime();
            }
        }
        
        public List<LightModel> GetListOfLightBullbs()
        {
            return _lightBullbModelList;
        }

        public LightModel GetLightModel(int lightBulbID)
        {
            return _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID);
        }

        public void SetNewBulbStatus(int lightBulbID, bool lightBulbStatus, DateTime dateTime)
        {
             SetTime(lightBulbID, lightBulbStatus, dateTime);
            _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).LightStatus = lightBulbStatus;
        }
    }
}
