using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.ServiceModel.Channels;

/// <summary>
/// IDuplexService - 双工（Duplex）服务契约
/// CallbackContract - 双工（Duplex）服务的回调类型
/// </summary>
[ServiceContract(Namespace = "Silverlight20", CallbackContract = typeof(IDuplexClient))]
public interface IDuplexService
{
    /// <summary>
    /// 客户端向服务端发送消息的方法
    /// </summary>
    /// <param name="receivedMessage">客户端向服务端发送的消息 System.ServiceModel.Channels.Message</param>
    [OperationContract(IsOneWay = true)]
    void SendStockCode(Message receivedMessage);
}

/// <summary>
/// 双工（Duplex）服务的回调接口
/// </summary>
public interface IDuplexClient
{
    /// <summary>
    /// 客户端接收服务端发送过来的消息的方法
    /// </summary>
    /// <param name="returnMessage">服务端向客户端发送的消息 System.ServiceModel.Channels.Message</param>
    [OperationContract(IsOneWay = true)]
    void ReceiveStockMessage(Message returnMessage);
}
