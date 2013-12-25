using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace SilverlightDemoWebClient_Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BookHandler : IHttpHandler
    {

        public static readonly string[] PriceList = new string[]
        {
            "66.00",
            "78.30",
            "56.50",
            "28.80",
            "77.00"
        };

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.Write(PriceList[int.Parse(context.Request.QueryString["No"])]);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
