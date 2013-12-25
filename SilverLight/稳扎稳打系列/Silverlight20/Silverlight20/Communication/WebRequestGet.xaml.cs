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

using System.Threading;
using System.IO;

namespace Silverlight20.Communication
{
    public partial class WebRequestGet : UserControl
    {
        // 接收 GET 方式数据的 REST 服务
        string _url = "http://localhost:3036/REST.svc/Users/json";

        // 异常信息
        string _exception = "";
        
        // SynchronizationContext - 同步上下文管理类
        SynchronizationContext _syncContext;

        public WebRequestGet()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            // SynchronizationContext.Current - 当前线程的同步上下文
            _syncContext = SynchronizationContext.Current;

            /*
             * HttpWebRequest - 对指定的 URI 发出请求
             *     HttpWebRequest.Create(uri) - 初始化一个 WebRequest
             *     HttpWebRequest.BeginGetResponse(AsyncCallback callback, Object state) - 开始对指定 URI 资源做异步请求
             *         AsyncCallback callback - System.AsyncCallback 委托。异步操作完成时调用的回调方法
             *         Object state - 包含此异步请求的对象。即相应的 HttpWebRequest 对象
             *     HttpWebRequest.Abort() - 取消该异步请求
             *     HttpWebRequest.Accept - HTTP 头的 Accept  部分
             *     HttpWebRequest.ContentType - HTTP 头的 ContentType 部分
             *     HttpWebRequest.Headers - HTTP 头的 key/value 对集合
             *     HttpWebRequest.Method - HTTP 方法（只支持GET和POST）
             *     HttpWebRequest.RequestUri - 所请求的 URI
             *     HttpWebRequest.HaveResponse - 是否接收到了指定 URI 的响应
             *     HttpWebRequest.AllowReadStreamBuffering - 是否对从 Internet 资源接收的数据做缓冲处理。默认值为true，将数据缓存在客户端内存中，以便随时被应用程序读取
             */

            HttpWebRequest request = WebRequest.Create(
                new Uri(_url, UriKind.Absolute)) as HttpWebRequest;
            request.Method = "GET";
            request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
        }

        private void ResponseCallback(IAsyncResult result)
        {
            // IAsyncResult.AsyncState - AsyncCallback 传过来的对象
            HttpWebRequest request = result.AsyncState as HttpWebRequest;

            WebResponse response = null;

            try
            {
                // HttpWebRequest.EndGetResponse(IAsyncResult) - 结束对指定 URI 资源做异步请求
                //     返回值为 WebResponse 对象
                response = request.EndGetResponse(result) as HttpWebResponse;
            }
            catch (Exception ex)
            {
                _exception = ex.ToString();
            }

            // SynchronizationContext.Post(SendOrPostCallback d, Object state) - 将异步消息发送到该同步上下文中
            //     SendOrPostCallback d - System.Threading.SendOrPostCallback 委托
            //     Object state - 需要传递的参数
            _syncContext.Post(GetResponse, response);
        }

        private void GetResponse(object state)
        {
            /*
             * HttpWebResponse - 对指定的 URI 做出响应
             *     GetResponseStream() - 获取响应的数据流
             */

            HttpWebResponse response = state as HttpWebResponse;

            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    lblMsg.Text = sr.ReadToEnd();
                }
            }
            else
            {
                lblMsg.Text = _exception;
            }
        }
    }
}
