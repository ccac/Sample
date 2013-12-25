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

using System.ServiceModel;
using System.Runtime.Serialization;

namespace SilverlightDemoWCF_Web
{
    [DataContract]
    public class Post
    {
        public Post(int id,string title,string author)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }
    }
}
