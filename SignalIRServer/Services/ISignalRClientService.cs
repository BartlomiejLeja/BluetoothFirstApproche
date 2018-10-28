using System.Threading.Tasks;

namespace SignalIRServer.Services
{
    public interface ISignalRClientService
    {
        Task Connect();

        Task InvokeSendSchedulePlan(string schedulePlanJson);
    }
}
