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

using System.IO;

namespace Silverlight20.Communication
{
    public partial class Json : UserControl
    {
        public Json()
        {
            InitializeComponent();

            // 演示如何处理 JSON（对象）
            JsonDemo();

            // 演示如何处理 JSON（集合）
            JsonDemo2();
        }

        /// <summary>
        /// 演示如何处理 JSON（对象）
        /// </summary>
        void JsonDemo()
        {
            // REST 服务的 URL
            Uri uri = new Uri("http://localhost:3036/REST.svc/User/webabcd/json", UriKind.Absolute);

            // 实例化 WebClient
            System.Net.WebClient client = new System.Net.WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(json_DownloadStringCompleted);
            client.DownloadStringAsync(uri);

            txtMsgJson.Text = "读取 JSON（对象） 数据中。。。";
        }

        void json_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 发生错误的话，则打印出来
                txtMsgJson.Text = e.Error.ToString();
                return;
            }

            // 将获得的字符串转换为 JSON（对象）
            var buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
            var ms = new MemoryStream(buffer);
            var jsonObject = System.Json.JsonObject.Load(ms) as System.Json.JsonObject;

            txtMsgJson.Text = e.Result + "\r\n";
            // 解析 JSON（对象）
            txtMsgJson.Text += string.Format("姓名: {0}, 生日: {1}",
                (string)jsonObject["Name"],
                ((DateTime)jsonObject["DayOfBirth"]).ToString("yyyy-MM-dd"));

            /* 
             * 总结：
             * JsonObject - 一个具有零或多个 key-value 对的无序集合。继承自抽象类 JsonValue
             *     JsonObject.Load(Stream) - 将指定的字符串流反序列化为 JSON 对象（CLR可用的类型）
             *     JsonObject[key] - JsonObject 索引器，获取 JsonObject 的指定key的value
             *     JsonObject.ContainsKey(key) - JsonObject 对象内是否具有指定的key
             */
        }

        /// <summary>
        /// 演示如何处理 JSON（集合）
        /// </summary>
        void JsonDemo2()
        {
            // REST 服务的 URL
            Uri uri = new Uri("http://localhost:3036/REST.svc/Users/json", UriKind.Absolute);

            // 实例化 WebClient
            System.Net.WebClient client = new System.Net.WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(json2_DownloadStringCompleted);
            client.DownloadStringAsync(uri);

            txtMsgJson2.Text = "读取 JSON（集合） 数据中。。。";
        }

        void json2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 发生错误的话，则打印出来
                txtMsgJson2.Text = e.Error.ToString();
                return;
            }

            // 将获得的字符串转换为 JSON（集合）
            var buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
            var ms = new MemoryStream(buffer);
            var jsonArray = System.Json.JsonArray.Load(ms) as System.Json.JsonArray;

            txtMsgJson2.Text = e.Result + "\r\n";
            txtMsgJson2.Text += string.Format("姓名: {0}, 生日: {1}",
                (string)jsonArray.First()["Name"],
                ((DateTime)jsonArray.Single(p => p["Name"] == "webabcd02")["DayOfBirth"]).ToString("yyyy-MM-dd"));

            /* 
             * 总结：
             * JsonArray - 一个具有零或多个 JsonValue（抽象类，JsonObject和JsonArray都继承自此类） 对象的有序序列
             *     JsonArray.Load(Stream) - 将指定的字符串流反序列化为 JSON 对象（CLR可用的类型）
             *     JsonArray[index] - JsonArray 索引器，获取 JsonArray 的指定索引的 JsonValue
             *     JsonArray 支持 LINQ
             */
        }
    }
}
