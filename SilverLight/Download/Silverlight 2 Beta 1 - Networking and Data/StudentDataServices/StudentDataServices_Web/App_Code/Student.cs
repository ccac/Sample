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
using Microsoft.Data.Web;
/// <summary>
/// Summary description for Student
/// </summary>

public class Student
{
    [DataWebKey]
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}
