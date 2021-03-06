﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerDiagnostics.ModulesService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="ModuleSummary", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class ModuleSummary : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string NameField;
        
        private string VersionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Version {
            get {
                return this.VersionField;
            }
            set {
                if ((object.ReferenceEquals(this.VersionField, value) != true)) {
                    this.VersionField = value;
                    this.RaisePropertyChanged("Version");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.ServiceModel.ServiceContractAttribute()]
    public interface IModules {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IModules/GetModules", ReplyAction="http://tempuri.org/IModules/GetModulesResponse")]
        System.IAsyncResult BeginGetModules(System.AsyncCallback callback, object asyncState);
        
        ServerDiagnostics.ModulesService.ModuleSummary[] EndGetModules(System.IAsyncResult result);
    }
    
    public interface IModulesChannel : ServerDiagnostics.ModulesService.IModules, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    public partial class GetModulesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetModulesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public ServerDiagnostics.ModulesService.ModuleSummary[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((ServerDiagnostics.ModulesService.ModuleSummary[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    public partial class ModulesClient : System.ServiceModel.ClientBase<ServerDiagnostics.ModulesService.IModules>, ServerDiagnostics.ModulesService.IModules {
        
        private BeginOperationDelegate onBeginGetModulesDelegate;
        
        private EndOperationDelegate onEndGetModulesDelegate;
        
        private System.Threading.SendOrPostCallback onGetModulesCompletedDelegate;
        
        private static System.ServiceModel.Channels.Binding defaultBinding = new System.ServiceModel.BasicHttpBinding();
        
        private static System.ServiceModel.EndpointAddress defaultAddress = new System.ServiceModel.EndpointAddress("http://localhost:8082/ServerDiagnostics_Web/Modules.svc");
        
        public ModulesClient() : 
                this(defaultBinding, defaultAddress) {
        }
        
        public ModulesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<GetModulesCompletedEventArgs> GetModulesCompleted;
        
        System.IAsyncResult ServerDiagnostics.ModulesService.IModules.BeginGetModules(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetModules(callback, asyncState);
        }
        
        ServerDiagnostics.ModulesService.ModuleSummary[] ServerDiagnostics.ModulesService.IModules.EndGetModules(System.IAsyncResult result) {
            return base.Channel.EndGetModules(result);
        }
        
        private System.IAsyncResult OnBeginGetModules(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((ServerDiagnostics.ModulesService.IModules)(this)).BeginGetModules(callback, asyncState);
        }
        
        private object[] OnEndGetModules(System.IAsyncResult result) {
            ServerDiagnostics.ModulesService.ModuleSummary[] retVal = ((ServerDiagnostics.ModulesService.IModules)(this)).EndGetModules(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetModulesCompleted(object state) {
            if ((this.GetModulesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetModulesCompleted(this, new GetModulesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetModulesAsync() {
            this.GetModulesAsync(null);
        }
        
        public void GetModulesAsync(object userState) {
            if ((this.onBeginGetModulesDelegate == null)) {
                this.onBeginGetModulesDelegate = new BeginOperationDelegate(this.OnBeginGetModules);
            }
            if ((this.onEndGetModulesDelegate == null)) {
                this.onEndGetModulesDelegate = new EndOperationDelegate(this.OnEndGetModules);
            }
            if ((this.onGetModulesCompletedDelegate == null)) {
                this.onGetModulesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetModulesCompleted);
            }
            base.InvokeAsync(this.onBeginGetModulesDelegate, null, this.onEndGetModulesDelegate, this.onGetModulesCompletedDelegate, userState);
        }
        
        protected override ServerDiagnostics.ModulesService.IModules CreateChannel() {
            return new ModulesClientChannel(this);
        }
        
        private class ModulesClientChannel : ChannelBase<ServerDiagnostics.ModulesService.IModules>, ServerDiagnostics.ModulesService.IModules {
            
            public ModulesClientChannel(System.ServiceModel.ClientBase<ServerDiagnostics.ModulesService.IModules> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetModules(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetModules", _args, callback, asyncState);
                return _result;
            }
            
            public ServerDiagnostics.ModulesService.ModuleSummary[] EndGetModules(System.IAsyncResult result) {
                object[] _args = new object[0];
                ServerDiagnostics.ModulesService.ModuleSummary[] _result = ((ServerDiagnostics.ModulesService.ModuleSummary[])(base.EndInvoke("GetModules", _args, result)));
                return _result;
            }
        }
    }
}
