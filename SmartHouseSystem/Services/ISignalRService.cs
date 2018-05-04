using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
    public interface ISignalRService
    {
      Task Connect(IWiFiService wiFiService);
      Task InvokeSendMethod(string isOn);
    }
}
