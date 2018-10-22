using System.ComponentModel;
using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
    public interface ISignalRService
    {
      Task ConnectionBuilder( IChartService chartService, ILightService lightService);
      Task InvokeTurnOnLight(bool isOn, int lightNumber);
    
      event PropertyChangedEventHandler CurrentConnectionStatePropertyChanged;
      event PropertyChangedEventHandler LightsListLoadedPropertyChanged;
    }
}
