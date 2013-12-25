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

using Silverlight20.WCFServiceReference;
using System.Threading;
using System.ServiceModel;

namespace Silverlight20.Communication
{
    public partial class WCF : UserControl
    {
        SynchronizationContext _syncContext;

        /// <summary>
        /// 演示 Silverlight 调用 WCF 服务
        /// </summary>
        public WCF()
        {
            InitializeComponent();

            // 代理的配置信息在配置文件中，UI线程上的异步调用
            Demo();

            // 代理的配置信息在程序中指定，UI线程上的异步调用
            Demo2();

            // 后台线程（非UI线程）上的异步调用
            Demo3();
        }

        void Demo()
        {
            /*             
             * 服务名Client - 系统自动生成的代理类
             *     方法名Completed - 调用指定的方法完成后所触发的事件
             *     方法名Async(参数1, 参数2 ..., object 用户标识) - 异步调用指定的方法
             *     Abort() - 取消调用
             */

            WCFServiceClient client = new WCFServiceClient();
            client.GetUserCompleted += new EventHandler<GetUserCompletedEventArgs>(client_GetUserCompleted);
            client.GetUserAsync("webabcd");
        }

        void Demo2()
        {
            /*
             * 服务名Client - 其构造函数可以动态地指定代理的配置信息（Silverlight 2.0 调用 WCF 只支持 BasicHttpBinding）
             */

            WCFServiceClient client = new WCFServiceClient(new BasicHttpBinding(), new EndpointAddress("http://localhost:3036/WCFService.svc"));
            client.GetUserCompleted += new EventHandler<GetUserCompletedEventArgs>(client_GetUserCompleted);
            client.GetUserAsync("webabcd2");
        }

        void client_GetUserCompleted(object sender, GetUserCompletedEventArgs e)
        {
            /*
             * 方法名CompletedEventArgs.Error - 该异步操作期间是否发生了错误
             * 方法名CompletedEventArgs.Result - 异步操作返回的结果。本例为 User 类型
             * 方法名CompletedEventArgs.UserState - 用户标识
             */

            if (e.Error != null)
            {
                lblMsg.Text += e.Error.ToString() + "\r\n";
                return;
            }

            if (e.Cancelled != true)
            {
                OutputResult(e.Result);
            }
        }

        void Demo3()
        {
            // UI 线程
            _syncContext = SynchronizationContext.Current;

            /*
             * ChannelFactory<T>.CreateChannel() - 创建 T 类型的信道
             * 服务名.Begin方法名() - 后台线程上异步调用指定方法（最后一个参数为 代理对象）
             */

            WCFService client = new ChannelFactory<WCFService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:3036/WCFService.svc")).CreateChannel();
            client.BeginGetUser("webabcd3", new AsyncCallback(ResponseCallback), client);
        }

        private void ResponseCallback(IAsyncResult result)
        {
            WCFService client = result.AsyncState as WCFService;

            // 服务名.End方法名() - 获取在后台线程（非UI线程）上异步调用的结果
            User user = client.EndGetUser(result);

            // 调用 UI 线程
            _syncContext.Post(GetResponse, user);
        }

        private void GetResponse(object state)
        {
            OutputResult(state as User);
        }


        /// <summary>
        /// 输出异步调用 WCF 服务的方法后返回的结果
        /// </summary>
        /// <param name="user"></param>
        void OutputResult(User user)
        {
            lblMsg.Text += string.Format("姓名：{0}；生日：{1}\r\n",
                user.Name,
                user.DayOfBirth.ToString("yyyy-MM-dd"));
        }
    }
}