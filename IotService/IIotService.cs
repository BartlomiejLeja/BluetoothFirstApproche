using System.ServiceModel;

namespace IotService
{
    [ServiceContract(CallbackContract =typeof(ICallback))]
    public interface IIotService
    {
        [OperationContract(IsOneWay = true)]
        void SendPirStatusToAllClients(string pirStatus);

        [OperationContract]
        void RegisterClients();

        [OperationContract]
        void UnregisterClients(string sessionId);

        [OperationContract]
        void GetPirStatusFromClient(string pirStatus);
        
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void SendPirStatusCallBack(string pirStatus);
    } 
}
