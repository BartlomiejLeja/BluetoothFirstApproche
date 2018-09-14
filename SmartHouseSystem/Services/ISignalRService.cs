using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface ISignalRService
    {
      Task ConnectionBuilder( IChartService chartService, ILightService lightService);
      Task InvokeSendStaticticData(bool status);
      Task InvokeTurnOnLight(bool isOn, int lightNumber);
      Task InvokeCheckStatusOfLights(bool x);
      SignalRService.ConnectionState ConnectionState1 { get; set; }
      event PropertyChangedEventHandler PropertyChanged;
    }
}
