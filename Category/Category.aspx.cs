using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication1;
public partial class Category_Category : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtdt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DataList();
            }
            catch (Exception W) { }
        }
    }
    private void DataList()
    {
        // String from = txtdt.Text.ToString();
        // String to = txtdt1.Text.ToString();

        // String[] StrPart = from.Split('/');

        // String[] StrPart1 = to.Split('/');

        int isActive = 0;
        if (chkisactive.Checked)
        {
            isActive = 1;
        }

        string query = "SELECT CategoryID,CategoryName,CategoryDescription,IsActive,CreatedOn FROM Category where isnull(IsDeleted,0)= 0 AND ISNULL(ISActive,0) = " + isActive + 
                        " order by Sequence";
        //" and convert(date,CreatedOn,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,CreatedOn,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "' " 

        DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvCategorylist.DataSource = dtbannerlist;
            gvCategorylist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvCategorylist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}