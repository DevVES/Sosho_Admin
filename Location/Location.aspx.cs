using System;
using System.Data;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Location_Location : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtdt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

        string query = "SELECT Id,Location,zipcode,State,District AS City,IsActive,CreatedOn FROM [dbo].[Zipcode] where isnull(IsDeleted,0)=0  and convert(date,CreatedOn,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,CreatedOn,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'  order by CreatedOn desc ";

        DataTable dtlocationlist = dbc.GetDataTable(query);
        if (dtlocationlist.Rows.Count > 0)
        {
            gvLocationlist.DataSource = dtlocationlist;
            gvLocationlist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvLocationlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}