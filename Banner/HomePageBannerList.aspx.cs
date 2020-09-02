using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Banner_HomePageBannerList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                txtdt.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                txtdt1.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                DataList();
            }
            catch (Exception W) { }
        }
    }
    private void DataList()
    {
        String from = txtdt.Text.ToString();
        String to = txtdt1.Text.ToString();

        String[] StrPart = from.Split('/');
      
        String[] StrPart1 = to.Split('/');

        string query = "SELECT  [Id],[Title],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [SalebhaiOnePage].[dbo].[HomepageBanner] where IsDeleted=0  and StartDate>=CONVERT(DATETIME, '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + " 00:00:00', 102) and StartDate<=CONVERT(DATETIME, '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + " 23:59:59', 102)  order by EndDate desc ";
        

       DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvbannerlist.DataSource = dtbannerlist;
            gvbannerlist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvbannerlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvbannerlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}