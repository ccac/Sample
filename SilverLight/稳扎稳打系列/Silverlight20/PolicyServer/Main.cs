using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Sockets;
using System.IO;
using System.Net;

namespace PolicyServer
{
    public partial class Main : Form
    {
        // 客户端 socket 发送到服务端的对策略文件的请求信息
        private readonly string _policyRequestString = "<policy-file-request/>";

        private Socket _listener; // 服务端监听的 socket
        private byte[] _policyBuffer; // 服务端策略文件的 buffer
        private byte[] _requestBuffer; // 客户端 socket 发送的请求信息的 buffer

        private int _received; // 接收到的信息字节数

        private bool _flag = false; // 标志位。服务端是否要处理传入的连接

        System.Threading.SynchronizationContext _syncContext;

        public Main()
        {
            InitializeComponent();

            _flag = true;

            lblStatus.Text = "PolicyServer状态：启动";
            lblStatus.ForeColor = Color.Green;

            // 启动 PolicyServer
            StartupPolicyServer();

            // UI 线程
            _syncContext = System.Threading.SynchronizationContext.Current;
        }

        private void btnStartup_Click(object sender, EventArgs e)
        {
            _flag = true;

            lblStatus.Text = "PolicyServer状态：启动";
            lblStatus.ForeColor = Color.Green;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _flag = false;

            lblStatus.Text = "PolicyServer状态：暂停";
            lblStatus.ForeColor = Color.Red;
        }

        /// <summary>
        /// 启动 PolicyServer
        /// </summary>
        private void StartupPolicyServer()
        {
            string policyFile = Path.Combine(Application.StartupPath, "clientaccesspolicy.xml");

            using (FileStream fs = new FileStream(policyFile, FileMode.Open, FileAccess.Read))
            {
                // 将策略文件的内容写入 buffer
                _policyBuffer = new byte[fs.Length];
                fs.Read(_policyBuffer, 0, _policyBuffer.Length);
            }

            // 初始化 socket ， 然后与端口绑定， 然后对端口进行监听
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 943)); // socket 请求策略文件使用 943 端口
            _listener.Listen(100);

            // 开始接受客户端传入的连接
            _listener.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        private void OnClientConnect(IAsyncResult result)
        {
            if (!_flag)
            {
                // PolicyServer 停用的话，则不再处理传入的连接
                _listener.BeginAccept(new AsyncCallback(OnClientConnect), null);
                return;
            }
                 
            Socket client; // 客户端发过来的 socket

            try
            {
                // 完成接受客户端传入的连接的这个异步操作，并返回客户端连入的 socket
                client = _listener.EndAccept(result);
            }
            catch (SocketException)
            {
                return;
            }

            _requestBuffer = new byte[_policyRequestString.Length];
            _received = 0;

            try
            {
                // 开始接收客户端传入的数据
                client.BeginReceive(_requestBuffer, 0, _policyRequestString.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);
            }
            catch (SocketException)
            {
                // socket 出错则关闭客户端 socket
                client.Close();
            }

            // 继续开始接受客户端传入的连接
            _listener.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }


        private void OnReceive(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;

            try
            {
                // 完成接收数据的这个异步操作，并计算累计接收的数据的字节数
                _received += client.EndReceive(result);

                if (_received < _policyRequestString.Length)
                {
                    // 没有接收到完整的数据，则继续开始接收
                    client.BeginReceive(_requestBuffer, _received, _policyRequestString.Length - _received, SocketFlags.None, new AsyncCallback(OnReceive), client);
                    return;
                }

                // 把接收到的数据转换为字符串
                string request = System.Text.Encoding.UTF8.GetString(_requestBuffer, 0, _received);

                if (StringComparer.InvariantCultureIgnoreCase.Compare(request, _policyRequestString) != 0)
                {
                    // 如果接收到的数据不是“<policy-file-request/>”，则关闭客户端 socket
                    client.Close();
                    return;
                }

                // 开始向客户端发送策略文件的内容
                client.BeginSend(_policyBuffer, 0, _policyBuffer.Length, SocketFlags.None, new AsyncCallback(OnSend), client);
            }

            catch (SocketException)
            {
                // socket 出错则关闭客户端 socket
                client.Close();
            }
        }

        private void OnSend(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;

            try
            {
                // 完成将信息发送到客户端的这个异步操作
                client.EndSend(result);

                // 获取客户端的ip地址及端口号，并显示
                _syncContext.Post(ResultCallback, client.LocalEndPoint.ToString());
            }
            finally
            {
                // 关闭客户端 socket
                client.Close();
            }
        }

        void ResultCallback(object result)
        {
            // 输出客户端的ip地址及端口号
            txtMsg.Text += result.ToString() + "\r\n";
        }
    }
}
