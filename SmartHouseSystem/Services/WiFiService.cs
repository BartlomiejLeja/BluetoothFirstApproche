using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SmartHouseSystem.Services
{
    public class WiFiService :IWiFiService
    {
        public async Task SendHttpRequestAsync(int state)
        {
           HttpClient httpClient = new HttpClient();
           var headers = httpClient.DefaultRequestHeaders;
           var httpResponseBody = String.Empty;

           Uri requestUri = new Uri($"http://192.168.1.113/control?cmd=GPIO,13,{state.ToString()}");

           var httpResponse = new HttpResponseMessage();
            Debug.WriteLine("TestService");
            try
           {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();  
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            Debug.WriteLine(httpResponseBody);
        }
    }
}
