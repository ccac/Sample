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

namespace SilverlightDemoJSON_Web
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
