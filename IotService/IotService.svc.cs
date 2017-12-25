using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace IotService
{
    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class IotService : IIotService
    {
        private static Dictionary<string, ICallback> clients = new Dictionary<string, ICallback>();
     
        public void SendPirStatusToAllClients(string pirStatus)
        {
                foreach (var client in clients)
                {
                    client.Value.SendPirStatusCallBack(pirStatus);
                }    
        }

        public void RegisterClients()
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            var sessionId = OperationContext.Current.SessionId;
            if (!clients.ContainsKey(sessionId))
            {
                clients.Add(sessionId, callback);
            }
        }

        public void UnregisterClients(string sessionId)
        {
            lock (clients)
            {
                if (clients.ContainsKey(sessionId))
                    clients.Remove(sessionId);
            }
        }

        public void GetPirStatusFromClient(string pirStatus)
        {
            SendPirStatusToAllClients(pirStatus);
        }
    }
}
