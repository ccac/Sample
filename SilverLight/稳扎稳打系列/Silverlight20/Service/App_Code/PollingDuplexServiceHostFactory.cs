using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;

/* 以下部分摘自文档 */

// 服务 svc 文件的 Factory 要指定为此类
public class PollingDuplexServiceHostFactory : ServiceHostFactoryBase
{
    public override ServiceHostBase CreateServiceHost(string constructorString,
        Uri[] baseAddresses)
    {
        return new PollingDuplexSimplexServiceHost(baseAddresses);
    }
}

class PollingDuplexSimplexServiceHost : ServiceHost
{
    public PollingDuplexSimplexServiceHost(params System.Uri[] addresses)
    {
        base.InitializeDescription(typeof(DuplexService), new UriSchemeKeyedCollection(addresses));
    }

    protected override void InitializeRuntime()
    {
        // 配置 WCF 服务与 Silverlight 客户端之间的 Duplex 通信
        // Silverlight 客户端定期轮询网络层上的服务，并检查回调信道上由服务端发送的所有新的消息
        // 该服务会将回调信道上的由服务端发送的所有消息进行排队，并在客户端轮询服务时将这些消息传递到该客户端

        PollingDuplexBindingElement pdbe = new PollingDuplexBindingElement()
        {
            // ServerPollTimeout - 轮询超时时间
            // InactivityTimeout - 服务端与客户端在此超时时间内无任何消息交换的情况下，服务会关闭其会话

            ServerPollTimeout = TimeSpan.FromSeconds(3),
            InactivityTimeout = TimeSpan.FromMinutes(1)
        };

        // 为服务契约（service contract）添加一个终结点（endpoint）
        // Duplex 服务仅支持 Soap11
        this.AddServiceEndpoint(
            typeof(IDuplexService),
            new CustomBinding(
                pdbe,
                new TextMessageEncodingBindingElement(
                    MessageVersion.Soap11,
                    System.Text.Encoding.UTF8),
                new HttpTransportBindingElement()),
                "");

        base.InitializeRuntime();
    }
}
