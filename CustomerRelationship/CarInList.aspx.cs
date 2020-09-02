using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class CustomerRelationship_CarInList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int WorkshopId = 0;
                int.TryParse(Request.Cookies["TUser"]["WorkshopId"], out WorkshopId);
                dbConnection dbcon = new dbConnection();
                clsDataSourse db = new clsDataSourse();
                startdate.Value = dbcon.getindiantime().ToString("dd-MMM-yyyy");
                enddate.Value = dbcon.getindiantime().ToString("dd-MMM-yyyy");
                if (Request.QueryString["sdate"] != null)
                {
                    startdate.Value = Request.QueryString["sdate"];
                }
                if (Request.QueryString["edate"] != null)
                {
                    enddate.Value = Request.QueryString["edate"];
                }
                DataTable dt = db.CarInList(startdate.Value, enddate.Value, false, WorkshopId.ToString());
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        catch (Exception aa)
        {

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int WorkshopId = 0;
            int.TryParse(Request.Cookies["TUser"]["WorkshopId"], out WorkshopId);
            dbConnection dbcon = new dbConnection();
            clsDataSourse db = new clsDataSourse();
            DataTable dt = db.CarInList(startdate.Value, enddate.Value, false, WorkshopId.ToString());
            GridView1.DataSource = dt;
            GridView1.DataBind();
            startdate.Value = DateTime.Parse(startdate.Value).ToString("dd-MMM-yyyy");
            enddate.Value = DateTime.Parse(enddate.Value).ToString("dd-MMM-yyyy");
        }
        catch (Exception aa)
        {

        }
    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
}