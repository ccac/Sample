using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;

/// <summary>
/// 提供 REST 服务的类
/// 注：Silverlight只支持 GET 和 POST
/// </summary>
[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class REST
{
    /// <summary>
    /// 用于演示返回 JSON（对象） 的 REST 服务
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [OperationContract]
    [WebGet(UriTemplate = "User/{name}/json", ResponseFormat = WebMessageFormat.Json)]
    public User HelloJson(string name)
    {
        return new User { Name = name, DayOfBirth = new DateTime(1980, 2, 14) };
    }

    /// <summary>
    /// 用于演示返回 JSON（集合） 的 REST 服务
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebGet(UriTemplate = "Users/json", ResponseFormat = WebMessageFormat.Json)]
    public List<User> HelloJson2()
    {
        return new List<User> 
        { 
            new User(){ Name = "webabcd01", DayOfBirth = new DateTime(1980, 1, 1) },
            new User(){ Name = "webabcd02", DayOfBirth = new DateTime(1980, 2, 2) },
            new User(){ Name = "webabcd03", DayOfBirth = new DateTime(1980, 3, 3) },
        };
    }

    /// <summary>
    /// 用于演示返回 XML（对象） 的 REST 服务
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [OperationContract]
    [WebGet(UriTemplate = "User/{name}/xml", ResponseFormat = WebMessageFormat.Xml)]
    public User HelloXml(string name)
    {
        return new User { Name = name, DayOfBirth = new DateTime(1980, 2, 14) };
    }

    /// <summary>
    /// 用于演示返回 XML（集合） 的 REST 服务
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebGet(UriTemplate = "Users/xml", ResponseFormat = WebMessageFormat.Xml)]
    public List<User> HelloXml2()
    {
        return new List<User> 
        { 
            new User(){ Name = "webabcd01", DayOfBirth = new DateTime(1980, 1, 1) },
            new User(){ Name = "webabcd02", DayOfBirth = new DateTime(1980, 2, 2) },
            new User(){ Name = "webabcd03", DayOfBirth = new DateTime(1980, 3, 3) },
        };
    }

    /// <summary>
    /// 用于演示以字符串的形式上传数据的 REST 服务
    /// </summary>
    /// <param name="fileName">上传的文件名</param>
    /// <param name="stream">POST 过来的数据</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke(UriTemplate = "UploadString/?fileName={fileName}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
    public bool UploadString(string fileName, Stream stream)
    {
        // 文件的服务端保存路径
        string path = Path.Combine("C:\\", fileName);

        try
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                // 将 POST 过来的被 Base64 编码过字符串传换成 byte[]
                byte[] buffer = Convert.FromBase64String(sr.ReadToEnd());

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // 将文件写入到服务端
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 用于演示以流的形式上传数据的 REST 服务
    /// </summary>
    /// <param name="fileName">上传的文件名</param>
    /// <param name="stream">POST 过来的数据（流的方式）</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke(UriTemplate = "UploadStream/?fileName={fileName}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
    public bool UploadStream(string fileName, Stream stream)
    {
        // 文件的服务端保存路径
        string path = Path.Combine("C:\\", fileName);

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = new byte[4096];
                int count = 0;

                // 每 POST 过来 4096 字节的数据，往服务端写一次
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 用于演示接收 POST 过来数据的 REST 服务
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke(UriTemplate = "PostUser", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
    public User HelloUser()
    {
        string name = System.Web.HttpContext.Current.Request["name"];

        return new User { Name = name, DayOfBirth = new DateTime(1980, 2, 14) };
    }
}

[ServiceContract]
public interface IPolicyProvider
{
    /// <summary>
    /// 获取策略文件内容的流
    /// </summary>
    /// <returns></returns>
    [OperationContract, WebGet(UriTemplate = "/ClientAccessPolicy.xml")]
    Stream ProvidePolicy();
}
