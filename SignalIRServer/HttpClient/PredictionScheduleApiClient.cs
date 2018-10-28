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
                BaseAddress = new Uri("http://localhost:58295/")
            };
        }

        public async Task<string> GetSchedule()
        {
            var res = await _client.GetAsync("api/prediction/1/1/1/10/30");
            string result = null;
            if (res.IsSuccessStatusCode)
            {
                result = res.Content.ReadAsStringAsync().Result;
            }

            return result;
        }

        public async Task<Uri> CreateProductAsync(LightBulbDbModel lightBulbDbModel)
        {
            var response = await _client.PostAsync("api/LightBulb", new JsonContent(lightBulbDbModel));
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

    }
}
