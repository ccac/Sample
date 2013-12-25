using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using System.Collections.Generic;

namespace SilverlightDemoWebService_Web
{
    /// <summary>
    /// Summary description for BlogService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BlogService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public Post[] GetPosts()
        {
            List<Post> list = new List<Post>() { 
                new Post{ Id=1, Title="一步一步学Silverlight 2系列（13）：数据与通信之WebRequest", Author="TerryLee" },
                new Post{ Id=2, Title="一步一步学Silverlight 2系列（12）：数据与通信之WebClient", Author="TerryLee" },
                new Post{ Id=3, Title="一步一步学Silverlight 2系列（11）：数据绑定", Author="TerryLee" },
                new Post{ Id=4, Title="一步一步学Silverlight 2系列（10）：使用用户控件", Author="TerryLee" },
                new Post{ Id=5, Title="一步一步学Silverlight 2系列（9）：使用控件模板", Author="TerryLee" },
                new Post{ Id=6, Title="一步一步学Silverlight 2系列（8）：使用样式封装控件观感", Author="TerryLee" }
            };

            return list.ToArray();
        }
    }
}
