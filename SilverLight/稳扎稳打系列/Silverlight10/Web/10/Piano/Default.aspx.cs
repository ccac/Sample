using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class _10_Piano_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Data.MusicBookDataContext mbdc = new Data.MusicBookDataContext();
        Data.MusicBook mb = new Data.MusicBook();

        mb.Name = txtName.Text;
        mb.Details = txtInput.Text;

        mbdc.MusicBook.InsertOnSubmit(mb);
        mbdc.SubmitChanges();

        txtName.Text = "";
        txtInput.Text = "";

        GridView1.DataBind();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtName.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[3].Text;
        txtInput.Text = ((Label)GridView1.Rows[GridView1.SelectedIndex].Cells[4].FindControl("lblDetails")).Text;
    }
}
