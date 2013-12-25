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

using System.Net.Sockets;
using System.Text;

namespace Silverlight20.Communication
{
    public partial class SocketClient : UserControl
    {
        // 信息结束符，用于判断是否完整地读取了用户发送的信息（要与服务端的信息结束符相对应）
        private string _endMarker = "^";

        // 客户端 Socket
        private Socket _socket;

        // Socket 异步操作对象
        private SocketAsyncEventArgs _sendEventArgs;

        public SocketClient()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化姓名和需要发送的默认文字
            txtName.Text = "匿名用户" + new Random().Next(0, 9999).ToString().PadLeft(4, '0');
            txtInput.Text = "hi";

            // 实例化 Socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 实例化 SocketAsyncEventArgs ，用于对 Socket 做异步操作，很方便
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            // 服务器的 EndPoint
            args.RemoteEndPoint = new DnsEndPoint("wanglei-pc", 4518);
            // 异步操作完成后执行的事件
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnSocketConnectCompleted);

            // 异步连接服务端
            _socket.ConnectAsync(args);
        }

        private void OnSocketConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            // 设置数据缓冲区
            byte[] response = new byte[1024];
            e.SetBuffer(response, 0, response.Length);

            // 修改 SocketAsyncEventArgs 对象的异步操作完成后需要执行的事件
            e.Completed -= new EventHandler<SocketAsyncEventArgs>(OnSocketConnectCompleted);
            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnSocketReceiveCompleted);

            // 异步地从服务端 Socket 接收数据
            _socket.ReceiveAsync(e);

            // 构造一个 SocketAsyncEventArgs 对象，用于用户向服务端发送消息
            _sendEventArgs = new SocketAsyncEventArgs();
            _sendEventArgs.RemoteEndPoint = e.RemoteEndPoint;

            string data = "";
            if (!_socket.Connected)
                data = "无法连接到服务器。。。请刷新后再试。。。";
            else
                data = "成功地连接上了服务器。。。";

            WriteText(data);
        }

        private void OnSocketReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                // 将接收到的数据转换为字符串
                string data = UTF8Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);

                WriteText(data);
            }
            catch (Exception ex)
            {
                WriteText(ex.ToString());
            }

            // 继续异步地从服务端 Socket 接收数据
            _socket.ReceiveAsync(e);
        }

        private void WriteText(string data)
        {
            // 在聊天文本框中输出指定的信息，并将滚动条滚到底部
            this.Dispatcher.BeginInvoke(
                delegate
                {
                    txtChat.Text += data + "\r\n";
                    scrollChat.ScrollToVerticalOffset(txtChat.ActualHeight);
                }
            );
        }

        private void SendData()
        {
            if (_socket.Connected)
            {
                // 设置需要发送的数据的缓冲区
                _sendEventArgs.BufferList =
                    new List<ArraySegment<byte>>() 
                    { 
                        new ArraySegment<byte>(UTF8Encoding.UTF8.GetBytes(txtName.Text + "：" + txtInput.Text + _endMarker)) 
                    };

                // 异步地向服务端 Socket 发送消息
                _socket.SendAsync(_sendEventArgs);
            }
            else
            {
                txtChat.Text += "无法连接到服务器。。。请刷新后再试。。。\r\n";
                _socket.Close();
            }

            txtInput.Focus();
            txtInput.Text = "";
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendData();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            // 按了回车键就向服务端发送数据
            if (e.Key == Key.Enter)
                SendData();
        }
    }
}
