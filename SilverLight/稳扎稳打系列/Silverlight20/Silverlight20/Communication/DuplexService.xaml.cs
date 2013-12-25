using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.IO;

namespace Silverlight20.Communication
{
    public partial class DuplexService : UserControl
    {
        SynchronizationContext _syncContext;

        // 是否接收服务端发送过来的消息
        bool _status = true;

        public DuplexService()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            _status = true;

            // UI 线程
            _syncContext = SynchronizationContext.Current;

            PollingDuplexHttpBinding binding = new PollingDuplexHttpBinding()
            {
                // InactivityTimeout - 服务端与客户端在此超时时间内无任何消息交换的情况下，服务会关闭其会话
                InactivityTimeout = TimeSpan.FromMinutes(1)
            };

            // 构造 IDuplexSessionChannel 的信道工厂
            IChannelFactory<IDuplexSessionChannel> factory =
                binding.BuildChannelFactory<IDuplexSessionChannel>(new BindingParameterCollection());

            // 打开信道工厂
            IAsyncResult factoryOpenResult =
                factory.BeginOpen(new AsyncCallback(OnOpenCompleteFactory), factory);

            if (factoryOpenResult.CompletedSynchronously)
            {
                // 如果信道工厂被打开的这个 异步操作 已经被 同步完成 则执行下一步
                CompleteOpenFactory(factoryOpenResult);
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _status = false;
        }

        void OnOpenCompleteFactory(IAsyncResult result)
        {
            // 该异步操作已被同步完成的话则不做任何操作，反之则执行下一步
            if (result.CompletedSynchronously)
                return;
            else
                CompleteOpenFactory(result);
        }

        void CompleteOpenFactory(IAsyncResult result)
        {
            IChannelFactory<IDuplexSessionChannel> factory = result.AsyncState as IChannelFactory<IDuplexSessionChannel>;

            // 完成异步操作，以打开信道工厂
            factory.EndOpen(result);

            // 在信道工厂上根据指定的地址创建信道
            IDuplexSessionChannel channel =
                factory.CreateChannel(new EndpointAddress("http://localhost:3036/DuplexService.svc"));

            // 打开信道
            IAsyncResult channelOpenResult =
                channel.BeginOpen(new AsyncCallback(OnOpenCompleteChannel), channel);

            if (channelOpenResult.CompletedSynchronously)
            {
                // 如果信道被打开的这个 异步操作 已经被 同步完成 则执行下一步
                CompleteOpenChannel(channelOpenResult);
            }
        }

        void OnOpenCompleteChannel(IAsyncResult result)
        {
            // 该异步操作已被同步完成的话则不做任何操作，反之则执行下一步
            if (result.CompletedSynchronously)
                return;
            else
                CompleteOpenChannel(result);
        }

        void CompleteOpenChannel(IAsyncResult result)
        {
            IDuplexSessionChannel channel = result.AsyncState as IDuplexSessionChannel;

            // 完成异步操作，以打开信道
            channel.EndOpen(result);

            // 构造需要发送到服务端的 System.ServiceModel.Channels.Message （客户端终结点与服务端终结点之间的通信单元）
            Message message = Message.CreateMessage(
                channel.GetProperty<MessageVersion>(), // MessageVersion.Soap11 （Duplex 服务仅支持 Soap11）
                "Silverlight20/IDuplexService/SendStockCode", // Action 为请求的目的地（需要执行的某行为的路径）
                txtStockCode.Text);

            // 向目的地发送消息
            IAsyncResult resultChannel =
                channel.BeginSend(message, new AsyncCallback(OnSend), channel);

            if (resultChannel.CompletedSynchronously)
            {
                // 如果向目的地发送消息的这个 异步操作 已经被 同步完成 则执行下一步
                CompleteOnSend(resultChannel);
            }

            // 监听指定的信道，用于接收返回的消息
            ReceiveLoop(channel);
        }

        void OnSend(IAsyncResult result)
        {
            // 该异步操作已被同步完成的话则不做任何操作，反之则执行下一步
            if (result.CompletedSynchronously)
                return;
            else
                CompleteOnSend(result);
        }

        void CompleteOnSend(IAsyncResult result)
        {
            IDuplexSessionChannel channel = (IDuplexSessionChannel)result.AsyncState;

            // 完成异步操作，以完成向目的地发送消息的操作
            channel.EndSend(result);
        }

        void ReceiveLoop(IDuplexSessionChannel channel)
        {
            // 监听指定的信道，用于接收返回的消息
            IAsyncResult result = 
                channel.BeginReceive(new AsyncCallback(OnReceiveComplete), channel);

            if (result.CompletedSynchronously)
            {
                CompleteReceive(result);
            }
        }

        void OnReceiveComplete(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
                return;
            else
                CompleteReceive(result);
        }

        void CompleteReceive(IAsyncResult result)
        {
            IDuplexSessionChannel channel = (IDuplexSessionChannel)result.AsyncState;

            try
            {
                // 完成异步操作，以接收到服务端发过来的消息
                Message receivedMessage = channel.EndReceive(result);

                if (receivedMessage == null)
                {
                    // 服务端会话已被关闭
                    // 此时应该关闭客户端会话，或向服务端发送消息以启动一个新的会话
                }
                else
                {
                    // 将接收到的信息输出到界面上
                    string text = receivedMessage.GetBody<string>();
                    _syncContext.Post(WriteText, text + Environment.NewLine);

                    if (!_status)
                    {
                        // 关闭信道
                        IAsyncResult resultFactory =
                            channel.BeginClose(new AsyncCallback(OnCloseChannel), channel);

                        if (resultFactory.CompletedSynchronously)
                        {
                            CompleteCloseChannel(result);
                        }

                    }
                    else
                    {
                        // 继续监听指定的信道，用于接收返回的消息
                        ReceiveLoop(channel);
                    }
                }
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
        }

        void OnCloseChannel(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
                return;
            else
                CompleteCloseChannel(result);
        }

        void CompleteCloseChannel(IAsyncResult result)
        {
            IDuplexSessionChannel channel = (IDuplexSessionChannel)result.AsyncState;

            // 完成异步操作，以关闭信道
            channel.EndClose(result);
        }

        void WriteText(object text)
        {
            // 将信息打到界面上
            lblStockMessage.Text += (string)text;
        }
    }
}