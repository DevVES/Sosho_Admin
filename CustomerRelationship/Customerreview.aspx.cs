using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class CustomerRelationship_Customerreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                dbConnection dbcon = new dbConnection();
                clsDataSourse db = new clsDataSourse();
                startdate.Value = dbcon.getindiantime().AddDays(-3).ToString("dd-MMM-yyyy");
                enddate.Value = dbcon.getindiantime().AddDays(-3).ToString("dd-MMM-yyyy");
                //if (Request.QueryString["sdate"] != null)
                //{
                //    startdate.Value = Request.QueryString["sdate"];
                //}
                //if (Request.QueryString["edate"] != null)
                //{
                //    enddate.Value = Request.QueryString["edate"];
                //}
                DataTable dt = db.JobCardPaymentV6(false, false, false, false, false, false, startdate.Value, enddate.Value,
                false, false, false, false, false, false, false, false, false, false);

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
            dbConnection dbcon = new dbConnection();
            clsDataSourse db = new clsDataSourse();
            DataTable dt = db.JobCardPaymentV6(false, false, true, true, true, true, startdate.Value, enddate.Value,
                false, false, false, false, false, false, false, false, false, false);
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