using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Proxy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 获取某个 url 地址的 html 并在页面上输出

        string url = Request.QueryString["url"];

        System.Net.WebClient client = new System.Net.WebClient();
        client.Encoding = System.Text.Encoding.UTF8;

        Response.Write(client.DownloadString(url));
        Response.End();
    }
}
