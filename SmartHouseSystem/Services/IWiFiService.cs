﻿using System.ComponentModel;
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
    }
}
