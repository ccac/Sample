using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.ServiceModel.Channels;
using System.Threading;
using System.ServiceModel.Activation;
using System.IO;

/// <summary>
/// Duplex 服务的服务端的实现
/// 本文以客户端向服务端提交股票代码，服务端定时向客户端发送股票信息为例
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class DuplexService : IDuplexService
{
    IDuplexClient _client;
    bool _status = true;

    /// <summary>
    /// 客户端向服务端发送股票代码的方法
    /// </summary>
    /// <param name="receivedMessage">包含股票代码的 System.ServiceModel.Channels.Message </param>
    public void SendStockCode(Message receivedMessage)
    {
        // 获取当前上下文的回调信道
        _client = OperationContext.Current.GetCallbackChannel<IDuplexClient>();

        // 如果发生错误则不再执行
        OperationContext.Current.Channel.Faulted += new EventHandler(delegate { _status = false; });

        // 获取用户提交的股票代码
        string stockCode = receivedMessage.GetBody<string>();

        // 每3秒向客户端发送一次股票信息
        while (_status)
        {
            // 构造需要发送到客户端的 System.ServiceModel.Channels.Message
            // Duplex 服务仅支持 Soap11 ， Action 为请求的目的地（需要执行的某行为的路径）
            Message stockMessage = Message.CreateMessage(
                MessageVersion.Soap11,
                "Silverlight20/IDuplexService/ReceiveStockMessage",
                string.Format("StockCode: {0}; StockPrice: {1}; CurrentTime: {2}",
                    stockCode,
                    new Random().Next(1, 200),
                    DateTime.Now.ToString()));

            try
            {
                // 向客户端“推”数据
                _client.ReceiveStockMessage(stockMessage);
            }
            catch (Exception ex)
            {
                // 出错则记日志
                using (StreamWriter sw = new StreamWriter(@"C:\Silverlight_Duplex_Log.txt", true))
                {
                    sw.Write(ex.ToString());
                    sw.WriteLine();
                }
            }

            System.Threading.Thread.Sleep(3000);
        }
    }
}