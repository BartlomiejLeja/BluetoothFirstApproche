﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestClient.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IIotService", CallbackContract=typeof(TestClient.ServiceReference1.IIotServiceCallback))]
    public interface IIotService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IIotService/SendPirStatusToAllClients")]
        void SendPirStatusToAllClients();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IIotService/SendPirStatusToAllClients")]
        System.Threading.Tasks.Task SendPirStatusToAllClientsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIotService/RegisterClients", ReplyAction="http://tempuri.org/IIotService/RegisterClientsResponse")]
        void RegisterClients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIotService/RegisterClients", ReplyAction="http://tempuri.org/IIotService/RegisterClientsResponse")]
        System.Threading.Tasks.Task RegisterClientsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIotService/UnregisterClients", ReplyAction="http://tempuri.org/IIotService/UnregisterClientsResponse")]
        void UnregisterClients(string sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIotService/UnregisterClients", ReplyAction="http://tempuri.org/IIotService/UnregisterClientsResponse")]
        System.Threading.Tasks.Task UnregisterClientsAsync(string sessionId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIotServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IIotService/SendPirStatusCallBack")]
        void SendPirStatusCallBack(string pirStatus);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIotServiceChannel : TestClient.ServiceReference1.IIotService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IotServiceClient : System.ServiceModel.DuplexClientBase<TestClient.ServiceReference1.IIotService>, TestClient.ServiceReference1.IIotService {
        
        public IotServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public IotServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public IotServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public IotServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public IotServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void SendPirStatusToAllClients() {
            base.Channel.SendPirStatusToAllClients();
        }
        
        public System.Threading.Tasks.Task SendPirStatusToAllClientsAsync() {
            return base.Channel.SendPirStatusToAllClientsAsync();
        }
        
        public void RegisterClients() {
            base.Channel.RegisterClients();
        }
        
        public System.Threading.Tasks.Task RegisterClientsAsync() {
            return base.Channel.RegisterClientsAsync();
        }
        
        public void UnregisterClients(string sessionId) {
            base.Channel.UnregisterClients(sessionId);
        }
        
        public System.Threading.Tasks.Task UnregisterClientsAsync(string sessionId) {
            return base.Channel.UnregisterClientsAsync(sessionId);
        }
    }
}
