using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
    public interface IWiFiService
    {
        Task SendHttpRequestAsync(int state);
    }
}
