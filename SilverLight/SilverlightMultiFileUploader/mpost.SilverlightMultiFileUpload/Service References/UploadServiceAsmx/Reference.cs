﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 2.0.5.0
// 
namespace mpost.SilverlightMultiFileUpload.UploadServiceAsmx {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UploadServiceAsmx.SilverlightUploadServiceSoap")]
    public interface SilverlightUploadServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.IAsyncResult BeginHelloWorld(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequest request, System.AsyncCallback callback, object asyncState);
        
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse EndHelloWorld(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/StoreFileAdvanced", ReplyAction="*")]
        System.IAsyncResult BeginStoreFileAdvanced(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequest request, System.AsyncCallback callback, object asyncState);
        
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse EndStoreFileAdvanced(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/CancelUpload", ReplyAction="*")]
        System.IAsyncResult BeginCancelUpload(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequest request, System.AsyncCallback callback, object asyncState);
        
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse EndCancelUpload(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class StoreFileAdvancedRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="StoreFileAdvanced", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequestBody Body;
        
        public StoreFileAdvancedRequest() {
        }
        
        public StoreFileAdvancedRequest(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class StoreFileAdvancedRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fileName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public byte[] data;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int dataLength;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string parameters;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public bool firstChunk;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public bool lastChunk;
        
        public StoreFileAdvancedRequestBody() {
        }
        
        public StoreFileAdvancedRequestBody(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk) {
            this.fileName = fileName;
            this.data = data;
            this.dataLength = dataLength;
            this.parameters = parameters;
            this.firstChunk = firstChunk;
            this.lastChunk = lastChunk;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class StoreFileAdvancedResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="StoreFileAdvancedResponse", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponseBody Body;
        
        public StoreFileAdvancedResponse() {
        }
        
        public StoreFileAdvancedResponse(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class StoreFileAdvancedResponseBody {
        
        public StoreFileAdvancedResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelUploadRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelUpload", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequestBody Body;
        
        public CancelUploadRequest() {
        }
        
        public CancelUploadRequest(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CancelUploadRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fileName;
        
        public CancelUploadRequestBody() {
        }
        
        public CancelUploadRequestBody(string fileName) {
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelUploadResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelUploadResponse", Namespace="http://tempuri.org/", Order=0)]
        public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponseBody Body;
        
        public CancelUploadResponse() {
        }
        
        public CancelUploadResponse(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class CancelUploadResponseBody {
        
        public CancelUploadResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface SilverlightUploadServiceSoapChannel : mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class SilverlightUploadServiceSoapClient : System.ServiceModel.ClientBase<mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap>, mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap {
        
        private BeginOperationDelegate onBeginHelloWorldDelegate;
        
        private EndOperationDelegate onEndHelloWorldDelegate;
        
        private System.Threading.SendOrPostCallback onHelloWorldCompletedDelegate;
        
        private BeginOperationDelegate onBeginStoreFileAdvancedDelegate;
        
        private EndOperationDelegate onEndStoreFileAdvancedDelegate;
        
        private System.Threading.SendOrPostCallback onStoreFileAdvancedCompletedDelegate;
        
        private BeginOperationDelegate onBeginCancelUploadDelegate;
        
        private EndOperationDelegate onEndCancelUploadDelegate;
        
        private System.Threading.SendOrPostCallback onCancelUploadCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SilverlightUploadServiceSoapClient() {
        }
        
        public SilverlightUploadServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SilverlightUploadServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightUploadServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightUploadServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<HelloWorldCompletedEventArgs> HelloWorldCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> StoreFileAdvancedCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CancelUploadCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.BeginHelloWorld(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginHelloWorld(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginHelloWorld(System.AsyncCallback callback, object asyncState) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequest inValue = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequest();
            inValue.Body = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequestBody();
            return ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).BeginHelloWorld(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.EndHelloWorld(System.IAsyncResult result) {
            return base.Channel.EndHelloWorld(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private string EndHelloWorld(System.IAsyncResult result) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse retVal = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).EndHelloWorld(result);
            return retVal.Body.HelloWorldResult;
        }
        
        private System.IAsyncResult OnBeginHelloWorld(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginHelloWorld(callback, asyncState);
        }
        
        private object[] OnEndHelloWorld(System.IAsyncResult result) {
            string retVal = this.EndHelloWorld(result);
            return new object[] {
                    retVal};
        }
        
        private void OnHelloWorldCompleted(object state) {
            if ((this.HelloWorldCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        public void HelloWorldAsync(object userState) {
            if ((this.onBeginHelloWorldDelegate == null)) {
                this.onBeginHelloWorldDelegate = new BeginOperationDelegate(this.OnBeginHelloWorld);
            }
            if ((this.onEndHelloWorldDelegate == null)) {
                this.onEndHelloWorldDelegate = new EndOperationDelegate(this.OnEndHelloWorld);
            }
            if ((this.onHelloWorldCompletedDelegate == null)) {
                this.onHelloWorldCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnHelloWorldCompleted);
            }
            base.InvokeAsync(this.onBeginHelloWorldDelegate, null, this.onEndHelloWorldDelegate, this.onHelloWorldCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.BeginStoreFileAdvanced(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginStoreFileAdvanced(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginStoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, System.AsyncCallback callback, object asyncState) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequest inValue = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequest();
            inValue.Body = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequestBody();
            inValue.Body.fileName = fileName;
            inValue.Body.data = data;
            inValue.Body.dataLength = dataLength;
            inValue.Body.parameters = parameters;
            inValue.Body.firstChunk = firstChunk;
            inValue.Body.lastChunk = lastChunk;
            return ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).BeginStoreFileAdvanced(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.EndStoreFileAdvanced(System.IAsyncResult result) {
            return base.Channel.EndStoreFileAdvanced(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private void EndStoreFileAdvanced(System.IAsyncResult result) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse retVal = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).EndStoreFileAdvanced(result);
        }
        
        private System.IAsyncResult OnBeginStoreFileAdvanced(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string fileName = ((string)(inValues[0]));
            byte[] data = ((byte[])(inValues[1]));
            int dataLength = ((int)(inValues[2]));
            string parameters = ((string)(inValues[3]));
            bool firstChunk = ((bool)(inValues[4]));
            bool lastChunk = ((bool)(inValues[5]));
            return this.BeginStoreFileAdvanced(fileName, data, dataLength, parameters, firstChunk, lastChunk, callback, asyncState);
        }
        
        private object[] OnEndStoreFileAdvanced(System.IAsyncResult result) {
            this.EndStoreFileAdvanced(result);
            return null;
        }
        
        private void OnStoreFileAdvancedCompleted(object state) {
            if ((this.StoreFileAdvancedCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.StoreFileAdvancedCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void StoreFileAdvancedAsync(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk) {
            this.StoreFileAdvancedAsync(fileName, data, dataLength, parameters, firstChunk, lastChunk, null);
        }
        
        public void StoreFileAdvancedAsync(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, object userState) {
            if ((this.onBeginStoreFileAdvancedDelegate == null)) {
                this.onBeginStoreFileAdvancedDelegate = new BeginOperationDelegate(this.OnBeginStoreFileAdvanced);
            }
            if ((this.onEndStoreFileAdvancedDelegate == null)) {
                this.onEndStoreFileAdvancedDelegate = new EndOperationDelegate(this.OnEndStoreFileAdvanced);
            }
            if ((this.onStoreFileAdvancedCompletedDelegate == null)) {
                this.onStoreFileAdvancedCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnStoreFileAdvancedCompleted);
            }
            base.InvokeAsync(this.onBeginStoreFileAdvancedDelegate, new object[] {
                        fileName,
                        data,
                        dataLength,
                        parameters,
                        firstChunk,
                        lastChunk}, this.onEndStoreFileAdvancedDelegate, this.onStoreFileAdvancedCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.BeginCancelUpload(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCancelUpload(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginCancelUpload(string fileName, System.AsyncCallback callback, object asyncState) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequest inValue = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequest();
            inValue.Body = new mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequestBody();
            inValue.Body.fileName = fileName;
            return ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).BeginCancelUpload(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap.EndCancelUpload(System.IAsyncResult result) {
            return base.Channel.EndCancelUpload(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private void EndCancelUpload(System.IAsyncResult result) {
            mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse retVal = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap)(this)).EndCancelUpload(result);
        }
        
        private System.IAsyncResult OnBeginCancelUpload(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string fileName = ((string)(inValues[0]));
            return this.BeginCancelUpload(fileName, callback, asyncState);
        }
        
        private object[] OnEndCancelUpload(System.IAsyncResult result) {
            this.EndCancelUpload(result);
            return null;
        }
        
        private void OnCancelUploadCompleted(object state) {
            if ((this.CancelUploadCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CancelUploadCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CancelUploadAsync(string fileName) {
            this.CancelUploadAsync(fileName, null);
        }
        
        public void CancelUploadAsync(string fileName, object userState) {
            if ((this.onBeginCancelUploadDelegate == null)) {
                this.onBeginCancelUploadDelegate = new BeginOperationDelegate(this.OnBeginCancelUpload);
            }
            if ((this.onEndCancelUploadDelegate == null)) {
                this.onEndCancelUploadDelegate = new EndOperationDelegate(this.OnEndCancelUpload);
            }
            if ((this.onCancelUploadCompletedDelegate == null)) {
                this.onCancelUploadCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCancelUploadCompleted);
            }
            base.InvokeAsync(this.onBeginCancelUploadDelegate, new object[] {
                        fileName}, this.onEndCancelUploadDelegate, this.onCancelUploadCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap CreateChannel() {
            return new SilverlightUploadServiceSoapClientChannel(this);
        }
        
        private class SilverlightUploadServiceSoapClientChannel : ChannelBase<mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap>, mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap {
            
            public SilverlightUploadServiceSoapClientChannel(System.ServiceModel.ClientBase<mpost.SilverlightMultiFileUpload.UploadServiceAsmx.SilverlightUploadServiceSoap> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginHelloWorld(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("HelloWorld", _args, callback, asyncState);
                return _result;
            }
            
            public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse EndHelloWorld(System.IAsyncResult result) {
                object[] _args = new object[0];
                mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse _result = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.HelloWorldResponse)(base.EndInvoke("HelloWorld", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginStoreFileAdvanced(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("StoreFileAdvanced", _args, callback, asyncState);
                return _result;
            }
            
            public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse EndStoreFileAdvanced(System.IAsyncResult result) {
                object[] _args = new object[0];
                mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse _result = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.StoreFileAdvancedResponse)(base.EndInvoke("StoreFileAdvanced", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginCancelUpload(mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("CancelUpload", _args, callback, asyncState);
                return _result;
            }
            
            public mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse EndCancelUpload(System.IAsyncResult result) {
                object[] _args = new object[0];
                mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse _result = ((mpost.SilverlightMultiFileUpload.UploadServiceAsmx.CancelUploadResponse)(base.EndInvoke("CancelUpload", _args, result)));
                return _result;
            }
        }
    }
}
