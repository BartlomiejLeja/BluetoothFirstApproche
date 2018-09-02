using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface ISignalRService
    {
      Task Connect(IWiFiService wiFiService, IChartService chartService, ILightService lightService);
    
      Task InvokeSendStaticticData(bool status);
      Task InvokeTurnOnLight(bool isOn, int lightNumber);
      Task InvokeCheckStatusOfLights(bool x);


    }
}
