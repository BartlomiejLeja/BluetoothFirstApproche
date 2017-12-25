﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IotService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/IotService")]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool BoolValueField;
        
        private string StringValueField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BoolValue
        {
            get
            {
                return this.BoolValueField;
            }
            set
            {
                this.BoolValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue
        {
            get
            {
                return this.StringValueField;
            }
            set
            {
                this.StringValueField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IService1", CallbackContract=typeof(IService1Callback))]
public interface IService1
{
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService1/NormalFunction")]
    void NormalFunction();
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService1/NormalFunction")]
    System.Threading.Tasks.Task NormalFunctionAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
    string GetData(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
    System.Threading.Tasks.Task<string> GetDataAsync(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService1/GetDataUsingDataContractResponse")]
    IotService.CompositeType GetDataUsingDataContract(IotService.CompositeType composite);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService1/GetDataUsingDataContractResponse")]
    System.Threading.Tasks.Task<IotService.CompositeType> GetDataUsingDataContractAsync(IotService.CompositeType composite);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/WelcomeMessage", ReplyAction="http://tempuri.org/IService1/WelcomeMessageResponse")]
    string WelcomeMessage(string name);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/WelcomeMessage", ReplyAction="http://tempuri.org/IService1/WelcomeMessageResponse")]
    System.Threading.Tasks.Task<string> WelcomeMessageAsync(string name);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendPirStatus", ReplyAction="http://tempuri.org/IService1/SendPirStatusResponse")]
    string SendPirStatus();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendPirStatus", ReplyAction="http://tempuri.org/IService1/SendPirStatusResponse")]
    System.Threading.Tasks.Task<string> SendPirStatusAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetPirStatus", ReplyAction="http://tempuri.org/IService1/GetPirStatusResponse")]
    string GetPirStatus(string status);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetPirStatus", ReplyAction="http://tempuri.org/IService1/GetPirStatusResponse")]
    System.Threading.Tasks.Task<string> GetPirStatusAsync(string status);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/RegisterClients", ReplyAction="http://tempuri.org/IService1/RegisterClientsResponse")]
    void RegisterClients();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/RegisterClients", ReplyAction="http://tempuri.org/IService1/RegisterClientsResponse")]
    System.Threading.Tasks.Task RegisterClientsAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UnregisterFromService", ReplyAction="http://tempuri.org/IService1/UnregisterFromServiceResponse")]
    void UnregisterFromService(string sessionId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UnregisterFromService", ReplyAction="http://tempuri.org/IService1/UnregisterFromServiceResponse")]
    System.Threading.Tasks.Task UnregisterFromServiceAsync(string sessionId);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IService1Callback
{
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService1/CallBackFunction")]
    void CallBackFunction(string pirStatus);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IService1Channel : IService1, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class Service1Client : System.ServiceModel.DuplexClientBase<IService1>, IService1
{
    
    public Service1Client(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance)
    {
    }
    
    public Service1Client(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName)
    {
    }
    
    public Service1Client(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public Service1Client(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public Service1Client(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, binding, remoteAddress)
    {
    }
    
    public void NormalFunction()
    {
        base.Channel.NormalFunction();
    }
    
    public System.Threading.Tasks.Task NormalFunctionAsync()
    {
        return base.Channel.NormalFunctionAsync();
    }
    
    public string GetData(int value)
    {
        return base.Channel.GetData(value);
    }
    
    public System.Threading.Tasks.Task<string> GetDataAsync(int value)
    {
        return base.Channel.GetDataAsync(value);
    }
    
    public IotService.CompositeType GetDataUsingDataContract(IotService.CompositeType composite)
    {
        return base.Channel.GetDataUsingDataContract(composite);
    }
    
    public System.Threading.Tasks.Task<IotService.CompositeType> GetDataUsingDataContractAsync(IotService.CompositeType composite)
    {
        return base.Channel.GetDataUsingDataContractAsync(composite);
    }
    
    public string WelcomeMessage(string name)
    {
        return base.Channel.WelcomeMessage(name);
    }
    
    public System.Threading.Tasks.Task<string> WelcomeMessageAsync(string name)
    {
        return base.Channel.WelcomeMessageAsync(name);
    }
    
    public string SendPirStatus()
    {
        return base.Channel.SendPirStatus();
    }
    
    public System.Threading.Tasks.Task<string> SendPirStatusAsync()
    {
        return base.Channel.SendPirStatusAsync();
    }
    
    public string GetPirStatus(string status)
    {
        return base.Channel.GetPirStatus(status);
    }
    
    public System.Threading.Tasks.Task<string> GetPirStatusAsync(string status)
    {
        return base.Channel.GetPirStatusAsync(status);
    }
    
    public void RegisterClients()
    {
        base.Channel.RegisterClients();
    }
    
    public System.Threading.Tasks.Task RegisterClientsAsync()
    {
        return base.Channel.RegisterClientsAsync();
    }
    
    public void UnregisterFromService(string sessionId)
    {
        base.Channel.UnregisterFromService(sessionId);
    }
    
    public System.Threading.Tasks.Task UnregisterFromServiceAsync(string sessionId)
    {
        return base.Channel.UnregisterFromServiceAsync(sessionId);
    }
}