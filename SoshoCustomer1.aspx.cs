using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class SoshoCustomer1 : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        getdata();
    }
    public void getdata()
    {
        try
        {

            //lblmsg.Text = "";
            //DataTable dtdata = dbc.GetDataTable("select Customer.Id,Customer.Mobile,(select top 1 concat(CustomerAddress.FirstName, ' ' ,CustomerAddress.LastName )from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as Name,(select top 1 CustomerAddress.Address from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as CustAddress,(select top 1 CustomerAddress.PinCode from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as CustPin from Customer Order By Customer.Id Desc");
            DataTable dtdata = dbc.GetDataTable("select Customer.Id,Customer.Mobile,(select top 1 concat(CustomerAddress.FirstName, ' ' ,CustomerAddress.LastName )from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as Name,(select top 1 CustomerAddress.Address from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as CustAddress,(select top 1 CustomerAddress.PinCode from CustomerAddress where CustomerAddress.CustomerId=Customer.Id)as CustPin from Customer where Customer.Mobile not in (select tblExcludedMobileNumbers.ExcludedNumber from tblExcludedMobileNumbers) Order By Customer.Id Desc");
            if (dtdata.Rows.Count > 0)
            {
                grd.Caption = "Total Product: " + dtdata.Rows.Count;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
            else
            {
                dtdata = new DataTable();
                grd.Caption = "Total Product: " + dtdata.Rows.Count;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    protected void btnCustomer_Click(object sender, EventArgs e)
    {
        try
        {

            Response.ClearContent();
            Response.Buffer = false;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grd.AllowPaging = false;
            getdata();
            grd.HeaderRow.Style.Add("background-color", "#FFFFFF");
            for (int i = 0; i < grd.HeaderRow.Cells.Count; i++)
            {
                grd.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
            }
            grd.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {

            //throw ex;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }  
    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grd.PageIndex = e.NewPageIndex;
        //getdata();
    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception E) { }
    }
}