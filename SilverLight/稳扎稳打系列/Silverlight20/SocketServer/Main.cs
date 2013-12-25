using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace SocketServer
{
    public partial class Main : Form
    {
        SynchronizationContext _syncContext;

        System.Timers.Timer _timer;

        // 信息结束符，用于判断是否完整地读取了用户发送的信息（要与客户端的信息结束符相对应）
        private string _endMarker = "^";

        // 服务端监听的 socket
        private Socket _listener;

        // 实例化 ManualResetEvent， 设置其初始状态为非终止状态（可入状态）
        private ManualResetEvent _connectDone = new ManualResetEvent(false);

        // 客户端 Socket 列表
        private List<ClientSocketPacket> _clientList = new List<ClientSocketPacket>();

        public Main()
        {
            InitializeComponent();

            // UI 线程
            _syncContext = SynchronizationContext.Current;

            // 启动后台线程去运行 Socket 服务
            Thread thread = new Thread(new ThreadStart(StartupSocketServer));
            thread.IsBackground = true;
            thread.Start();
        }

        private void StartupSocketServer()
        {
            // 每 10 秒运行一次计时器所指定的方法
            _timer = new System.Timers.Timer();
            _timer.Interval = 10000d;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();

            // 初始化 socket ， 然后与端口绑定， 然后对端口进行监听
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 4518)); // Silverlight 2.0 使用 Socket 只能连接 4502-4534 端口
            _listener.Listen(100);


            while (true)
            {
                // 重置 ManualResetEvent，由此线程来控制 ManualResetEvent，其它到这里来的线程请等待
                // 为求简单易懂，本例实际上只有主线程会在这里循环运行
                _connectDone.Reset();

                // 开始接受客户端传入的连接
                _listener.BeginAccept(new AsyncCallback(OnClientConnect), null);

                // 阻止当前线程，直到当前 ManualResetEvent 调用 Set 发出继续信号
                _connectDone.WaitOne();
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 每 10 秒给所有连入的客户端发送一次消息
            SendData(string.Format("webabcd 对所有人说：大家好！ 【信息来自服务端 {0}】", DateTime.Now.ToString("hh:mm:ss")));
        }

        private void OnClientConnect(IAsyncResult async)
        {
            // 当前 ManualResetEvent 调用 Set 以发出继续信号，从而允许继续执行一个或多个等待线程
            _connectDone.Set();

            ClientSocketPacket client = new ClientSocketPacket();
            // 完成接受客户端传入的连接的这个异步操作，并返回客户端连入的 socket
            client.Socket = _listener.EndAccept(async);

            // 将客户端连入的 Socket 放进客户端 Socket 列表
            _clientList.Add(client);


            SendData("一个新的客户端已经成功连入服务器。。。 【信息来自服务端】");


            try
            {
                // 开始接收客户端传入的数据
                client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), client);
            }
            catch (SocketException ex)
            {
                // 处理异常
                HandleException(client, ex);
            }
        }

        private void OnDataReceived(IAsyncResult async)
        {
            ClientSocketPacket client = async.AsyncState as ClientSocketPacket;

            int count = 0;

            try
            {
                // 完成接收数据的这个异步操作，并返回接收的字节数
                if (client.Socket.Connected)
                    count = client.Socket.EndReceive(async);
            }
            catch (SocketException ex)
            {
                HandleException(client, ex);
            }

            // 把接收到的数据添加进收到的字节集合内
            // 本例采用UTF8编码，中文占用3字节，英文占用1字节，缓冲区为32字节
            // 所以如果直接把当前缓冲区转成字符串的话可能会出现乱码，所以要等接收完用户发送的全部信息后再转成字符串
            foreach (byte b in client.Buffer.Take(count))
            {
                if (b == 0) continue; // 如果是空字节则不做处理

                client.ReceivedByte.Add(b);
            }

            // 把当前接收到的数据转换为字符串。用于判断是否包含自定义的结束符
            string receivedString = UTF8Encoding.UTF8.GetString(client.Buffer, 0, count);

            // 如果该 Socket 在网络缓冲区中没有排队的数据 并且 接收到的数据中有自定义的结束符时
            if (client.Socket.Connected && client.Socket.Available == 0 && receivedString.Contains(_endMarker))
            {
                // 把收到的字节集合转换成字符串（去掉自定义结束符）
                // 然后清除掉字节集合中的内容，以准备接收用户发送的下一条信息
                string content = UTF8Encoding.UTF8.GetString(client.ReceivedByte.ToArray());
                content = content.Replace(_endMarker, "");
                client.ReceivedByte.Clear();

                // 发送数据到所有连入的客户端，并在服务端做记录
                SendData(content);
                _syncContext.Post(ResultCallback, content);
            }

            try
            {
                // 继续开始接收客户端传入的数据
                if (client.Socket.Connected)
                    client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, 0, new AsyncCallback(OnDataReceived), client);
            }
            catch (SocketException ex)
            {
                HandleException(client, ex);
            }
        }

        /// <summary>
        /// 发送数据到所有连入的客户端
        /// </summary>
        /// <param name="data">需要发送的数据</param>
        private void SendData(string data)
        {
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);

            foreach (ClientSocketPacket client in _clientList)
            {
                if (client.Socket.Connected)
                {
                    try
                    {
                        // 如果某客户端 Socket 是连接状态，则向其发送数据
                        client.Socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnDataSent), client);
                    }
                    catch (SocketException ex)
                    {
                        HandleException(client, ex);
                    }
                }
                else 
                {
                    // 某 Socket 断开了连接的话则将其关闭，并将其清除出客户端 Socket 列表
                    // 也就是说每次向所有客户端发送消息的时候，都会从客户端 Socket 集合中清除掉已经关闭了连接的 Socket
                    client.Socket.Close();
                    _clientList.Remove(client);
                }
            }
        }

        private void OnDataSent(IAsyncResult async)
        {
            ClientSocketPacket client = async.AsyncState as ClientSocketPacket;

            try
            {
                // 完成将信息发送到客户端的这个异步操作
                if (client.Socket.Connected)
                    client.Socket.EndSend(async);
            }
            catch (SocketException ex)
            {
                HandleException(client, ex);
            }
        }

        /// <summary>
        /// 处理 SocketException 异常
        /// </summary>
        /// <param name="client">导致异常的 ClientSocketPacket</param>
        /// <param name="ex">SocketException</param>
        private void HandleException(ClientSocketPacket client, SocketException ex)
        {
            // 在服务端记录异常信息，关闭导致异常的 Socket，并将其清除出客户端 Socket 列表
            _syncContext.Post(ResultCallback, client.Socket.RemoteEndPoint.ToString() + " - " + ex.Message);
            client.Socket.Close();
            _clientList.Remove(client);
        }

        private void ResultCallback(object result)
        {
            // 输出相关信息
            txtMsg.Text += result.ToString() + "\r\n";
        }
    }
}