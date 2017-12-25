using System;
using System.ServiceModel;
using TestClient.ServiceReference1;

namespace TestClient
{
    class MyCallBack : IIotServiceCallback, IDisposable
    {
        IotServiceClient client;
        public void SendPirStatusCallBack(string pirStatus)
        {
            Console.WriteLine(pirStatus);
        }

        public void callService()
        {
            var context = new InstanceContext(this);
            client = new IotServiceClient(context);
            client.RegisterClients();
            client.SendPirStatusToAllClients();

        }

        public void Dispose()
        {
            client.Close();
        }
    }
}