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
using System.Collections.Generic;

/// <summary>
/// Summary description for StudentDataContainer
/// </summary>
public class StudentDataContainer
{
    public StudentDataContainer()
    {
        _list.Add(new Student { ID = 1, Name = "Alex", Age = 38, Address = "WA" });
        _list.Add(new Student { ID = 2, Name = "Chris", Age = 26, Address = "CA" });
        _list.Add(new Student { ID = 3, Name = "Vicky", Age = 22, Address = "PA" });
    }
    public IQueryable<Student> Students
    {
        get { return _list.AsQueryable(); }
    }
    List<Student> _list = new List<Student>();
}

