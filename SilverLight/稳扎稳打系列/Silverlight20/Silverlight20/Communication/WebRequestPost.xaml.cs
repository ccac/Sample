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
    public partial class WebRequestPost : UserControl
    {
        // 接收 POST 方式数据的 REST 服务
        string _url = "http://localhost:3036/REST.svc/PostUser";

        // 异常信息
        string _exception = "";

        // SynchronizationContext - 同步上下文管理类
        SynchronizationContext _syncContext;

        public WebRequestPost()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            _syncContext = SynchronizationContext.Current;

            HttpWebRequest request = WebRequest.Create(
                new Uri(_url, UriKind.Absolute)) as HttpWebRequest;

            request.Method = "POST";

            // BeginGetRequestStream(AsyncCallback callback, Object state) - 向指定的 URI 资源发送数据的流的异步请求
            //     AsyncCallback callback - System.AsyncCallback 委托
            //     Object state - 包含此异步请求的对象。即相应的 HttpWebRequest 对象
            IAsyncResult asyncResult = request.BeginGetRequestStream(
                new AsyncCallback(RequestStreamCallback), request);
        }

        private void RequestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";

            // HttpWebRequest.EndGetRequestStream(IAsyncResult) - 返回用于将数据写入某 URI 资源的 Stream
            Stream requestStream = request.EndGetRequestStream(result);

            StreamWriter streamWriter = new StreamWriter(requestStream);

            // byte[] postdata = System.Text.Encoding.UTF8.GetBytes("name=webabcd");
            // 多个参数用“&”分隔
            streamWriter.Write("name=webabcd");

            streamWriter.Close();

            request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
        }

        private void ResponseCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;

            WebResponse response = null;

            try
            {
                response = request.EndGetResponse(result);
            }
            catch (Exception ex)
            {
                _exception = ex.ToString();
            }

            // 调用 UI 线程
            _syncContext.Post(GetResponse, response);
        }

        private void GetResponse(object state)
        {
            /*
             * HttpWebResponse - 对指定的 URI 做出响应
             *     GetResponseStream() - 获取响应的数据流
             *     ContentLength - 接收的数据的内容长度
             *     ContentType - HTTP 头的 ContentType 部分
             *     Method - HTTP 方法
             *     ResponseUri - 响应该请求的 URI
             *     StatusCode - 响应状态 [System.Net.HttpStatusCode枚举]
             *         HttpStatusCode.OK - HTTP 状态为 200
             *         HttpStatusCode.NotFound - HTTP 状态为 404
             *     StatusDescription - 响应状态的说明
             */

            HttpWebResponse response = state as HttpWebResponse;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    lblMsg.Text = string.Format("接收的数据的内容长度：{0}\r\nHTTP 头的 ContentType 部分：{1}\r\nHTTP 方法：{2}\r\n响应该请求的 URI：{3}\r\n响应状态：{4}\r\n响应状态的说明：{5}\r\n响应的结果：{6}\r\n",
                        response.ContentLength,
                        response.ContentType,
                        response.Method,
                        response.ResponseUri,
                        response.StatusCode,
                        response.StatusDescription,
                        sr.ReadToEnd());
                }
            }
            else
            {
                lblMsg.Text = _exception;
            }
        }
    }
}