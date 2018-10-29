using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SignalIRServer.HttpClient;
using SignalIRServer.Model;
using SignalIRServer.Repository;
using System.Threading.Tasks;

namespace SignalIRServer.Services
{
    public class LightsService : ILightsService
    {
        private readonly List<LightBulbModel> _lightBullbModelList = new List<LightBulbModel>();
        private DateTime _compareValue= new DateTime(0001, 1, 1);
        private readonly ILogger _logger;
        private readonly ILightBulbRepository _lightBulbRepository;

        public LightsService(ILogger<LightsService> logger, ILightBulbRepository lightBulbRepository)
        {
            _lightBulbRepository = lightBulbRepository;
            _logger = logger;
            _lightBullbModelList.Add(new LightBulbModel(1, false, "First"));
            _lightBullbModelList.Add(new LightBulbModel(2, false, "Second"));
        }

        public async void SetTime(int lightBulbID, bool lightBulbStatus, DateTime dateTime)
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

                var lightBulbToSaveToDb = _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID);
                await _lightBulbRepository.Create(
                    new LightBulbDbModel()
                {
                    ID= lightBulbToSaveToDb.ID,
                    Name = lightBulbToSaveToDb.Name,
                    LightStatus = lightBulbToSaveToDb.LightStatus,
                    TimeOn = lightBulbToSaveToDb.TimeOn,
                    TimeOff = lightBulbToSaveToDb.TimeOff,
                    BulbOffTimeInMinutesPerDay = lightBulbToSaveToDb.BulbOffTimeInMinutesPerDay,
                    BulbOnTimeInMinutesPerDay = lightBulbToSaveToDb.BulbOnTimeInMinutesPerDay
                    }
                );

                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOn = new DateTime();
                _lightBullbModelList.First(lightBulb => lightBulb.ID == lightBulbID).TimeOff = new DateTime();
            }
        //    await SavePredictionModelToDbAsync(lightBulbID, dateTime, lightBulbStatus);
        }

        public async Task SavePredictionModelToDbAsync(int lightBulbID,DateTime dateTime,bool status)
        {
            var predictionUsageLightBulbModel = new PredictionUsageLightBulbModel()
            {
                LightBulbID = lightBulbID,IsOn = status ? 1:0,
                Day=(int)dateTime.DayOfWeek,Month= dateTime.Month,Time =(dateTime.Hour*60 + dateTime.Minute)
            };
            var httpClient = new PredictionScheduleApiClient();

            await httpClient.CreatePredictionModelAsync(predictionUsageLightBulbModel);
        }

        public List<LightBulbModel> GetListOfLightBullbs()
        {
            return _lightBullbModelList;
        }

        public LightBulbModel GetLightModel(int lightBulbID)
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
