using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;


namespace SmartHouseSystem.Services
{
    public class WiFiService :  IWiFiService, INotifyPropertyChanged
    {
        private HttpListener listener;
        private string cmd;
        private bool lightStatus;
        public string Cmd { get => cmd; set { cmd = value; NotifyPropertyChanged(nameof(cmd)); } }
    
        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(String propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        
        public async Task SendHttpRequestAsync(bool state)
        {
            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            var httpResponseBody = String.Empty;
            Uri requestUri;
        
            if(state) requestUri = new Uri($"http://192.168.1.109/control?cmd=GPIO,14,0");
            else requestUri = new Uri($"http://192.168.1.109/control?cmd=GPIO,14,1");

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
       
        public async Task <string> CheckStatusOfLight()
        {
            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            var httpResponseBody = String.Empty;
            Uri requestUri = new Uri($"http://192.168.1.109/control?cmd=STATUS,GPIO,14");

            var httpResponse = new HttpResponseMessage();
            Debug.WriteLine("Check Status Test");
            
            using (httpResponse = await httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
               
                return await httpResponse.Content.ReadAsStringAsync();
            } 
        }
          
        public async Task ListenHttpRequestsAsync()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://+:8080/");
            listener.Start();
            Debug.WriteLine("Listning started");
            Debug.WriteLine(listener.GetContextAsync());
            while (true)
            {
                var context = await listener.GetContextAsync();
                var response = context.Response;
                const string responseString = "<html><body>Hello world</body></html>";

                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;

                var output = response.OutputStream;

                output.Write(buffer, 0, buffer.Length);

                string[] urlRequestTab = (context.Request.Url).ToString().Split("=");
                Cmd = urlRequestTab[urlRequestTab.Length-1];

                Debug.WriteLine(cmd);
            }
        }
    }
}
