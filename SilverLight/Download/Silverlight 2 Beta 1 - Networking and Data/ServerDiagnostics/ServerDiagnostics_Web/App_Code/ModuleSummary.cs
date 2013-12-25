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
using System.Runtime.Serialization;

/// <summary>
/// Summary description for ModuleSummary
/// </summary>

[DataContract]
public class ModuleSummary
{
    [DataMember]
    public String Name { get; set; }

    [DataMember]
    public String Version {get; set;}
}
