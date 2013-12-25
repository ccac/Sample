using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace SilverlightDemoLiveSearch_Web
{
    public class SearchResultItem
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }
}
