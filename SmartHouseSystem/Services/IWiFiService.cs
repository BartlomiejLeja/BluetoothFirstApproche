using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
    public interface IWiFiService
    {
        Task SendHttpRequestAsync(bool state);
        Task ListenHttpRequestsAsync();
        Task<string> CheckStatusOfLight();
        string Cmd { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler PropertyChanged1;
        DateTime LightOnDataTime { get; set; }
        DateTime LightOffDataTime { get; set; }
    }
}
