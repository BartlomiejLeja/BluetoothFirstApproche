using System;
using System.Threading.Tasks;
using SignalIRServer.Model;


namespace SignalIRServer.HttpClient
{
    public class PredictionScheduleApiClient
    {
        private readonly System.Net.Http.HttpClient _client;
        public PredictionScheduleApiClient()
        {
            _client = new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri("smarthousemachinelearningv10020181028070832.azurewebsites.net/")
            };
        }

        public async Task<string> GetSchedule(int lightBulbId, int month, int day, int timeFrom, int timeTo)
        {
            var res = await _client.GetAsync($"api/prediction/{lightBulbId}/{month}/{day}/{timeFrom}/{timeTo}");
            string result = null;
            if (res.IsSuccessStatusCode)
            {
                result = res.Content.ReadAsStringAsync().Result;
            }

            return result;
        }

        public async Task<string> Train()
        {
            var res = await _client.GetAsync("api/prediction/train");
            string result = null;
            if (res.IsSuccessStatusCode)
            {
                result = res.Content.ReadAsStringAsync().Result;
            }

            return result;
        }

        public async Task<Uri> CreatePredictionModelAsync(PredictionUsageLightBulbModel lightBulbDbModel)
        {
            var response = await _client.PostAsync("api/LightBulb", new JsonContent(lightBulbDbModel));
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

    }
}
