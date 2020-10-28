using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Wallet_WalletHistoryDashboard : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string dtStart = "";
    string dtEnd = "", IsAdmin = "", sJurisdictionId = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                dtStart = dbc.getindiantime().ToString("dd/MMM/yyyy");
                dtEnd = dbc.getindiantime().ToString("dd/MMM/yyyy");

                txtDate.Text = dbc.getindiantime().AddDays(-1).ToString("dd-MM-yyyy");
                txtDate1.Text = dbc.getindiantime().ToString("dd-MM-yyyy");

                IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();

                Date();
                boxbind();

                string queryData = "select top 1 [Product].Name,[Product].Id, CONVERT(varchar(12),EndDate,107)+' '+CONVERT(varchar(20),EndDate,108) as Enddate1,StartDate,EndDate,sold  from Product  where StartDate <= '" + dbc.getindiantimeString(true) + "' and EndDate>= '" + dbc.getindiantimeString(true) + "' ";
                DataTable dttime = dbc.GetDataTable(queryData);

                if (dttime != null && dttime.Rows.Count > 0)
                {
                    string endtime = dttime.Rows[0]["Enddate1"].ToString();
                    string display = dttime.Rows[0]["Name"].ToString();
                    //lblenddate.Value = endtime;
                    string Id = dttime.Rows[0]["Id"].ToString();
                    string htmlbodystr = "";
                    htmlbodystr = (@"<div><a  href='Product/ManageProducts.aspx?Id=" + Id + "' target=\"_blank\">" + display + "</a> <p class=\"inline\">- Offer Expiring After:</p> <p id=\"demo\" class=\"inline\"></p></div>");
                    //divTest.InnerHtml = htmlbodystr;
                }
            }
            catch (Exception W) { ltrErr.Text = W.Message; }
        }
    }

    string todate = "";
    string fromdate = "";
    public void Date()
    {
        try
        {
            string fdt = txtDate.Text;
            string[] arr = fdt.Split('-');
            int day = 0;
            int month = 0;
            int year = 0;
            int.TryParse(arr[0].ToString(), out day);
            int.TryParse(arr[1].ToString(), out month);
            int.TryParse(arr[2].ToString(), out year);
            DateTime frmdt = new DateTime(year, month, day);
            fromdate = frmdt.ToString("dd-MMM-yyyy");

            string tdt = txtDate1.Text;
            string[] arr1 = tdt.Split('-');
            int day1 = 0;
            int month1 = 0;
            int year1 = 0;
            int.TryParse(arr1[0].ToString(), out day1);
            int.TryParse(arr1[1].ToString(), out month1);
            int.TryParse(arr1[2].ToString(), out year1);
            DateTime todt = new DateTime(year1, month1, day1);
            todate = todt.ToString("dd-MMM-yyyy");

        }
        catch (Exception ee)
        {
            //Logger.WriteCriticalLog("SummaryReport 114: exception:" + ee.Message + "::::::::" + ee.StackTrace);
            ltrErr.Text = ee.Message;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            boxbind();
        }
        catch (Exception ee)
        {
            ltrErr.Text = ee.Message;
        }
    }

    private void boxbind()
    {
        try
        {
            Date();
            string where = "";
            if (todate != null && fromdate != null && todate != "" && fromdate != "")
            {
                where = " where   Cr_date <= '" + todate + " 23:59:59' and Cr_date >= '" + fromdate + " 00:00:00' ";
                if (IsAdmin == "False")
                    where += " and JurisdictionID=" + sJurisdictionId;
            }
            else
            {
                where = " where   Cr_date <='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59' and Cr_date >='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00' ";
                if (IsAdmin == "False")
                    where += " and JurisdictionID=" + sJurisdictionId;
            }

            string qry = "SELECT Count(Id) AS NumberOfCount FROM tblWalletCustomerHistory " + where;
            DataTable dt = dbc.GetDataTable(qry);
            dbc.InsertLogs(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                ltrCreatedTransaction.Text = dt.Rows[0]["NumberOfCount"].ToString();
            }
            else
            {
                ltrCreatedTransaction.Text = "0";
            }

            string debitqry = "SELECT Count(Id) AS NumberOfCount FROM tblWalletCustomerHistory " + where +
                          " AND Dr_amount > 0 ";
            DataTable debitdt = dbc.GetDataTable(debitqry);
            dbc.InsertLogs(debitqry);
            if (debitdt != null && debitdt.Rows.Count > 0)
            {
                ltrHasgoneCount.Text = debitdt.Rows[0]["NumberOfCount"].ToString();
            }
            else
            {
                ltrHasgoneCount.Text = "0";
            }

            string creditqry = "SELECT Count(Id) AS NumberOfCount FROM tblWalletCustomerHistory " + where +
                          " AND Cr_amount > 0  AND Dr_amount = 0 ";
            DataTable creditdt = dbc.GetDataTable(creditqry);
            dbc.InsertLogs(creditqry);
            if (creditdt != null && creditdt.Rows.Count > 0)
            {
                ltrhascame.Text = creditdt.Rows[0]["NumberOfCount"].ToString();
            }
            else
            {
                ltrhascame.Text = "0";
            }

            string successqry = "SELECT Count(Id) AS NumberOfCount FROM tblWalletCustomerHistory " + where +
                          " AND balance = 0 ";
            DataTable successdt = dbc.GetDataTable(successqry);
            dbc.InsertLogs(successqry);
            if (successdt != null && successdt.Rows.Count > 0)
            {
                ltrissuccess.Text = successdt.Rows[0]["NumberOfCount"].ToString();
            }
            else
            {
                ltrissuccess.Text = "0";
            }
        }
        catch (Exception ee)
        {
            ltrErr.Text = ee.Message;
        }
    }
    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        var fromdate = txtDate.Text;
        var todate = txtDate1.Text;

        Response.Redirect("WalletDashboardList.aspx?id=1&startDt=" + fromdate + "&endDt=" + todate);

    }
    protected void lnkInvoice_Click(object sender, EventArgs e)
    {
        var fromdate = txtDate.Text;
        var todate = txtDate1.Text;

        Response.Redirect("WalletDashboardList.aspx?id=2&startDt=" + fromdate + "&endDt=" + todate);

    }
    protected void lnkShipped_Click(object sender, EventArgs e)
    {
        var fromdate = txtDate.Text;
        var todate = txtDate1.Text;

        Response.Redirect("WalletDashboardList.aspx?id=3&startDt=" + fromdate + "&endDt=" + todate);

    }
    protected void lnkWalletSuccess_Click(object sender, EventArgs e)
    {
        var fromdate = txtDate.Text;
        var todate = txtDate1.Text;

        Response.Redirect("WalletDashboardList.aspx?id=4&startDt=" + fromdate + "&endDt=" + todate);

    }
}