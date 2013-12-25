using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Caching;
using System.Xml;

namespace CacheDependency
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(Server.MapPath("Languages.xml"));
                Cache.Insert("LangCache",
                    xdoc,
                    new System.Web.Caching.CacheDependency(Server.MapPath("Languages.xml")),
                    DateTime.MaxValue,
                    new TimeSpan(),
                    System.Web.Caching.CacheItemPriority.Default,
                    new System.Web.Caching.CacheItemRemovedCallback(RefreshCache));
            }
        }

        public void RefreshCache(String k, Object v, CacheItemRemovedReason r)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(Server.MapPath("Languages.xml"));
            Cache.Insert("LangCache",
                xdoc,
                new System.Web.Caching.CacheDependency(Server.MapPath("Languages.xml")),
                DateTime.MaxValue,
                new TimeSpan(),
                System.Web.Caching.CacheItemPriority.Default,
                new System.Web.Caching.CacheItemRemovedCallback(RefreshCache));
        }
    }
}
