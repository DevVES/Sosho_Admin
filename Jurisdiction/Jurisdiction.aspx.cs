using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication1;

public partial class Jurisdiction_Jurisdiction : System.Web.UI.Page
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

        string query = "SELECT JM.JurisdictionID,JM.JurisdictionIncharge,JM.Contact,JM.EmalID,SM.StateName,CM.CityName,JM.Comments,JM.IsActive FROM JurisdictionMaster Jm INNER JOIN StateMaster sm on JM.StateId = sm.Id INNER JOIN CityMaster cm on JM.CityId = cm.Id where isnull(Jm.IsDeleted,0)=0  and convert(date,CreatedOn,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,CreatedOn,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'  order by CreatedOn desc ";

        DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvJurisdictionlist.DataSource = dtbannerlist;
            gvJurisdictionlist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvJurisdictionlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}