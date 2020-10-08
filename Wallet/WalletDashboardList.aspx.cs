using System;
using System.Data;
using WebApplication1;

public partial class Wallet_WalletDashboardList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    string dtStart = "";
    string dtEnd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var id = Request["id"].ToString();
                var startDate = Request["startDt"].ToString();
                var endDate = Request["endDt"].ToString();
                
                DataList(id, startDate,endDate);
            }
            catch (Exception W) { }
        }
    }
    
    private void DataList(string id, string startDate, string endDate)
    {
        string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
        string query = string.Empty;
        string[] arr = startDate.Split('-');
        int day = 0;
        int month = 0;
        int year = 0;
        int.TryParse(arr[0].ToString(), out day);
        int.TryParse(arr[1].ToString(), out month);
        int.TryParse(arr[2].ToString(), out year);
        DateTime frmdt = new DateTime(year, month, day);
        string fromdate = frmdt.ToString("dd-MMM-yyyy");

        string[] arr1 = endDate.Split('-');
        int day1 = 0;
        int month1 = 0;
        int year1 = 0;
        int.TryParse(arr1[0].ToString(), out day1);
        int.TryParse(arr1[1].ToString(), out month1);
        int.TryParse(arr1[2].ToString(), out year1);
        DateTime todt = new DateTime(year1, month1, day1);
        string todate = todt.ToString("dd-MMM-yyyy");
        string where = string.Empty;
        if (todate != null && fromdate != null && todate != "" && fromdate != "")
        {
            where = " where   Cr_date <= '" + todate + " 23:59:59' and Cr_date >= '" + fromdate + " 00:00:00' ";
        }
        else
        {
            where = " where   Cr_date <='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59' and Cr_date >='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00' ";
        }

        query = " SELECT[O].Id AS OrderId, [H].customer_id, H.Cr_date AS[Date], " +
                           " (case isnull(H.order_id, 0) when 0 then isnull(H.Cr_description, '') else 'Order Id ' + CAST(O.Id AS varchar) end) AS Description, " +
                           " H.Cr_amount, H.Dr_amount, H.Balance " +
                           " FROM tblWalletCustomerHistory H " +
                           " LEFT OUTER JOIN[Order] O ON H.customer_id = O.CustomerId  AND O.Id = H.order_id " +
                           where;
        if (id == "2")
        {
            query += " AND Dr_amount > 0 ";
        }
        if(id == "3")
        {
            query += " AND Cr_amount > 0  AND Dr_amount = 0 ";
        }
        if (id == "4")
        {
            query += " AND balance = 0 ";
        }
        query += " order by H.Id desc";
        DataTable dtwalletlist = dbc.GetDataTable(query);
        if (dtwalletlist.Rows.Count > 0)
        {
            gvwalletHistorylist.DataSource = dtwalletlist;
            gvwalletHistorylist.DataBind();
        }
    }
}