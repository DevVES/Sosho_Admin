using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
public partial class Home : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    //System.Data.DataTable dtaddresschnage = new System.Data.DataTable();
    //System.Data.DataTable dtorderfeed = new System.Data.DataTable();
    //System.Data.DataTable dtfeedbck = new System.Data.DataTable();
    string dtStart = "";
    string dtEnd = "", IsAdmin = "", sJurisdictionId = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        // int xyz = Convert.ToInt32("Abc");
        if (!IsPostBack)
        {
            try
            {
                dtStart = dbc.getindiantime().ToString("dd/MMM/yyyy");
                dtEnd = dbc.getindiantime().ToString("dd/MMM/yyyy");

                //  lblDateTime.Text = String.Format("{0:ddd, MMM d, yyyy}", dbc.getindiantime());


                txtDate.Text = dbc.getindiantime().AddDays(-1).ToString("dd-MM-yyyy");
                txtDate1.Text = dbc.getindiantime().ToString("dd-MM-yyyy");

                IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();

                Date();
                boxbind();

                LAST_5_Order();
                Last_5_RegisCust();
                Lst_5_Buy_with_alon_order();
                gr_lst_5_buy_with_2();
                gd_last_5Buy_with_6();

                gridbind();

                string queryData = "select top 1 [Product].Name,[Product].Id, CONVERT(varchar(12),EndDate,107)+' '+CONVERT(varchar(20),EndDate,108) as Enddate1,StartDate,EndDate,sold  from Product  where StartDate <= '" + dbc.getindiantimeString(true) + "' and EndDate>= '" + dbc.getindiantimeString(true) + "' ";
                DataTable dttime = dbc.GetDataTable(queryData);



                if (dttime != null && dttime.Rows.Count > 0)
                {
                    string endtime = dttime.Rows[0]["Enddate1"].ToString();
                    string display = dttime.Rows[0]["Name"].ToString();
                    lblenddate.Value = endtime;

                    string Id = dttime.Rows[0]["Id"].ToString();


                    string htmlbodystr = "";
                    htmlbodystr = (@"<div><a  href='Product/ManageProducts.aspx?Id=" + Id + "' target=\"_blank\">" + display + "</a> <p class=\"inline\">- Offer Expiring After:</p> <p id=\"demo\" class=\"inline\"></p></div>");
                    divTest.InnerHtml = htmlbodystr;

                }

            }
            catch (Exception W) { ltrErr.Text = W.Message; }
        }
    }
    public void AlertMsg(string msg)
    {
        //  ltrErr.Text += " " + msg;
        //string script = "<script type=\"text/javascript\">alert('" + msg + "');</script>";
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
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

    private void boxbind()
    {
        try
        {
            // dbc.InsertLogs("start find data");
            Date();
            string where = "";
            string where1 = "";
            string wherec = "";
            if (todate != null && fromdate != null && todate != "" && fromdate != "")
            {
                //  dbc.InsertLogs("start find data 1");
                where = " where   [Order].createdOnUtc <= '" + todate + " 23:59:59' and [Order].createdOnUtc >= '" + fromdate + " 00:00:00' ";
                if (IsAdmin == "False")
                    where += " and JurisdictionID=" + sJurisdictionId;

                where1 = " AND [Order].createdOnUtc <= '" + todate + " 23:59:59' and [Order].createdOnUtc >= '" + fromdate + " 00:00:00'";

                wherec = " where DOC <= '" + todate + " 23:59:59' AND DOC  >= '" + fromdate + " 00:00:00' ";
            }
            else
            {
                where = " where   [Order].createdOnUtc <='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59' and [Order].createdOnUtc>='" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00' ";
                if(IsAdmin == "False")
                    where += " and JurisdictionID=" + sJurisdictionId;

                where1 = " AND [Order].createdOnUtc <= '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59' and [Order].createdOnUtc >= '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00'";

                wherec = " where DOC <= '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59'  AND DOC  >= '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00' ";
            }
            // dbc.InsertLogs("start find data2");






            string qry = "Select  * from [Order] " + where;
            AlertMsg(qry);
            DataTable dt = dbc.GetDataTable(qry);
            dbc.InsertLogs(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                ltrReqVsGrn.Text = dt.Rows.Count.ToString();
            }
            else
            {
                ltrReqVsGrn.Text = "0";
            }
            //dbc.InsertLogs("start find data4");

            string q = "Select Count(Id) as totalCust FROM Customer " + wherec;
            AlertMsg(q);
            DataTable d = dbc.GetDataTable(q);
            if (d != null && d.Rows.Count > 0)
            {
                ltrcustomer.Text = d.Rows[0]["totalCust"].ToString();
            }
            else
            {
                ltrcustomer.Text = "0";
            }
            // dbc.InsertLogs("start find data5");

            string qr = "Select Count(BuyWith) as totalorder FROM [Order] where BuyWith=1 " + where1;
            AlertMsg(qr);
            DataTable dr = dbc.GetDataTable(qr);
            if (dr != null && dr.Rows.Count > 0)
            {
                Ltr1.Text = dr.Rows[0]["totalorder"].ToString();
                // dbc.InsertLogs(dr.Rows[0]["totalorder"].ToString());
            }
            else
            {
                Ltr1.Text = "0";
            }

            string s = "Select Count(BuyWith) as totc FROM [Order]where BuyWith=2 " + where1;
            AlertMsg(s);
            DataTable drt = dbc.GetDataTable(s);
            if (drt != null && drt.Rows.Count > 0)
            {
                Literal2.Text = drt.Rows[0]["totc"].ToString();
            }
            else
            {
                Literal2.Text = "0";
            }


            string r = "Select Count(BuyWith) as toc FROM [Order]where BuyWith=6 " + where1;
            AlertMsg(r);
            DataTable t = dbc.GetDataTable(r);
            if (t != null && t.Rows.Count > 0)
            {
                Ltr3.Text = t.Rows[0]["toc"].ToString();
            }
            else
            {
                Ltr3.Text = "0";
            }

        }
        catch (Exception ee)
        {
            ltrErr.Text = ee.Message;
            //throw;
        }
    }

    //private void TodaysProduct()
    //{
    //    try
    //    {
    //        string podqry = "SELECT [Product].Id AS PoductId, [Product].Name,[Product].StartDate,[Product].EndDate,[Product].Mrp,[Product].BuyWith1FriendExtraDiscount,[Product].BuyWith5FriendExtraDiscount  FROM [Product]  where StartDate < '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 00:00:00' and EndDate> '" + dbc.getindiantime().ToString("dd/MMM/yyyy") + " 23:59:59' ";
    //        DataTable Product = dbc.GetDataTable(podqry);
    //        if(Product != null && Product.Rows.Count>0)
    //        {

    //        }
    //    }
    //    catch (Exception)
    //    {
    //    }
    //}

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
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    public void LAST_5_Order()
    {
        try
        {
            string qry;
            qry = "Select Top(5) [Order].Id,CustomerAddress.FirstName as Name, CustomerAddress.MobileNo as MobileNumber,[Product].Name as ProductName, Concat((Convert(Varchar,[Order].CreatedOnUtc,106)),' ',(Convert(Varchar,[Order].CreatedOnUtc,108)))as OrderDate,[Order].TotalQTY from [Order] INNER JOIN OrderItem ON [Order].Id=OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN Customer ON Customer.Id =[Order].CustomerId INNER JOIN CustomerAddress ON Customer.id=CustomerAddress.CustomerId  ORDER BY [Order].Id DESC";
            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                // grd_Lst_5_Order.Caption = dt.Rows.Count + ": Rows Found";
                grd_Lst_5_Order.DataSource = dt;
                grd_Lst_5_Order.DataBind();
            }
            else
            {
                grd_Lst_5_Order.Caption = "No Data Found";
                grd_Lst_5_Order.DataSource = null;
                grd_Lst_5_Order.DataBind();
            }

        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    public void Last_5_RegisCust()
    {
        try
        {
            //INNER JOIN Customer ON CustomerAddress.CountryId=CountryMaster.Id INNER JOIN Customer ON CustomerAddress.StateId=StateMaster.Id INNER JOIN Customer ON CustomerAddress.CityId=CityMaster.Id
            string qry;
            qry = "Select DISTINCT  TOP(5) [Customer].Id, [Customer].Mobile As MobileNumber,CustomerAddress.FirstName AS Custname,(Select Top 1 B.DOC from CustomerAddress as B where B.CustomerId= [Customer].Id order by Id asc)as regdate,Concat((select top 1 Zipcode.[Location] from Zipcode where Zipcode.zipcode=[CustomerAddress].Pincode),'-',([CustomerAddress].PinCode)) as PinCode,(select iif(count([Order].CustomerId)>0,'Y','N') as IsOrder from [Order] where [Order].CustomerId=[Customer].Id)as IsOrder FROM  [Customer] INNER JOIN CustomerAddress ON Customer.Id = CustomerAddress.CustomerId  ORDER BY Customer.Id DESC  ";
            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                //grd_Lst_5_Regis.Caption = dt.Rows.Count + ": Rows Found";
                grd_Lst_5_Regis.DataSource = dt;
                grd_Lst_5_Regis.DataBind();
            }
            else
            {
                grd_Lst_5_Regis.Caption = "No Data Found";
                grd_Lst_5_Regis.DataSource = null;
                grd_Lst_5_Regis.DataBind();
            }
        }

        catch (Exception E)
        {
            ltrErr.Text = E.Message;

        }
    }
    public void Lst_5_Buy_with_alon_order()
    {
        try
        {
            string qry;
            qry = "Select Top(5)[Order].Id, [Product].Name as ProductName, CustomerAddress.FirstName as Name, CustomerAddress.MobileNo as MobileNumber,Concat((Convert(Varchar,[Order].CreatedOnUtc,106)),' ',(Convert(Varchar,[Order].CreatedOnUtc,108)))as OrderDate,[Order].TotalQTY from [Order] INNER JOIN OrderItem ON [Order].Id=OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN Customer ON Customer.id=[order].CustomerId INNER JOIN CustomerAddress ON Customer.Id=CustomerAddress.CustomerId where [Order].BuyWith<=1  ORDER BY [Order].Id DESC ";
            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                // gr_Lst_5_Buy_with_lon_order.Caption = dt.Rows.Count + ": Rows Found";
                gr_Lst_5_Buy_with_lon_order.DataSource = dt;
                gr_Lst_5_Buy_with_lon_order.DataBind();
            }
            else
            {
                gr_Lst_5_Buy_with_lon_order.Caption = "No Data Found";
                gr_Lst_5_Buy_with_lon_order.DataSource = null;
                gr_Lst_5_Buy_with_lon_order.DataBind();
            }
        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    public void gr_lst_5_buy_with_2()
    {
        try
        {
            string qry;
            qry = "Select Top(5)[Order].Id,[Product].Name as ProductName,CustomerAddress.FirstName as Name, CustomerAddress.MobileNo as MobileNumber,Concat((Convert(Varchar,[Order].CreatedOnUtc,106)),' ',(Convert(Varchar,[Order].CreatedOnUtc,108)))as OrderDate,[Order].TotalQTY from [Order] INNER JOIN OrderItem ON [Order].Id=OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN Customer ON Customer.id=[order].CustomerId INNER JOIN CustomerAddress ON Customer.Id=CustomerAddress.CustomerId where [Order].BuyWith=2  ORDER BY [Order].Id DESC";
            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                //grd_lst_5_buy_with_2.Caption = dt.Rows.Count + ": Rows Found";
                grd_lst_5_buy_with_2.DataSource = dt;
                grd_lst_5_buy_with_2.DataBind();
            }
            else
            {
                grd_lst_5_buy_with_2.Caption = "No Data Found";
                grd_lst_5_buy_with_2.DataSource = null;
                grd_lst_5_buy_with_2.DataBind();
            }
        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    public void gd_last_5Buy_with_6()
    {
        try
        {
            string qry;
            qry = "Select Top(5) [Order].Id,[Product].Name as ProductName,CustomerAddress.FirstName as Name, CustomerAddress.MobileNo as MobileNumber,Concat((Convert(Varchar,[Order].CreatedOnUtc,106)),' ',(Convert(Varchar,[Order].CreatedOnUtc,108)))as OrderDate,[Order].TotalQTY from [Order] INNER JOIN OrderItem ON [Order].Id=OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN Customer ON Customer.id=[order].CustomerId INNER JOIN CustomerAddress ON Customer.Id=CustomerAddress.CustomerId where [Order].BuyWith=6  ORDER BY [Order].Id DESC";
            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                //grd_last_5Buy_with_6.Caption = dt.Rows.Count + ": Rows Found";
                grd_last_5Buy_with_6.DataSource = dt;
                grd_last_5Buy_with_6.DataBind();
            }
            else
            {
                grd_last_5Buy_with_6.Caption = "No Data Found";
                grd_last_5Buy_with_6.DataSource = null;
                grd_last_5Buy_with_6.DataBind();
            }
        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    public void gridbind()
    {
        try
        {
            //string qry = "SELECT TOP (50) [Order].Id AS ordid, Customer.Mobile, [Order].AddressId, CustomerAddress.Address AS cadd, [Order].OrderStatusId, [Order].OrderTotal AS PaymentAmt, [Order].OrderMRP AS Totalamt, [Order].TotalQTY, [Order].BuyWith, [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, Product.Name, CustomerAddress.Id, CustomerAddress.CustomerId, CustomerAddress.FirstName AS Custname, CustomerAddress.LastName, CustomerAddress.TagId, CustomerAddress.CountryId, CustomerAddress.StateId, CustomerAddress.CityId, CustomerAddress.Address, CustomerAddress.Email, CustomerAddress.MobileNo, CustomerAddress.PinCode, CustomerAddress.DOC, CustomerAddress.DOM, CustomerAddress.IsDeleted, CustomerAddress.IsActive FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id WHERE ([Order].IsPaymentDone = 1) ORDER BY ordid DESC";

            string where = "";

            if (txtDate.Text != null && txtDate.Text != "" && txtDate1.Text != null && txtDate1.Text != "")
            {
                string fdate = txtDate.Text;
                string[] fdt = fdate.Split('-');
                int date = int.Parse(fdt[0].ToString());
                int month = int.Parse(fdt[1].ToString());
                int yrar = int.Parse(fdt[2].ToString());
                DateTime frmdt = new DateTime(yrar, month, date);

                string tdate = txtDate1.Text;
                string[] tdt = tdate.Split('-');
                int tdate1 = int.Parse(tdt[0].ToString());
                int tmonth = int.Parse(tdt[1].ToString());
                int tyrar = int.Parse(tdt[2].ToString());
                DateTime ftdate = new DateTime(tyrar, tmonth, tdate1);
                where = " AND [Order].createdOnUtc>=CONVERT(varchar,'" + frmdt.ToString("MM-dd-yyyy") + " 00:00:00',204) and [Order].createdOnUtc<=CONVERT(varchar,'" + ftdate.ToString("MM-dd-yyyy") + " 23:59:59',204)";
            }

            string qry = "SELECT TOP (50) [Order].Id AS ordid, Customer.Mobile, [Order].AddressId, CustomerAddress.Address AS cadd,CustomerAddress.PinCode,Concat((Convert(Varchar,[Order].CreatedOnUtc,106)),' ',(Convert(Varchar,[Order].CreatedOnUtc,108)))as OrderDate,[Order].OrderStatusId,[Order].OrderTotal AS PaymentAmt, [Order].OrderMRP AS Totalamt, [Order].TotalQTY, [Order].BuyWith, [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, Product.Name,OrderStatus.Name AS Ex,CustomerAddress.Id, CustomerAddress.CustomerId, CustomerAddress.FirstName AS Custname, CustomerAddress.LastName, CustomerAddress.TagId, CustomerAddress.CountryId, CustomerAddress.StateId, CustomerAddress.CityId, CustomerAddress.Address, CustomerAddress.Email, CustomerAddress.MobileNo, CustomerAddress.PinCode,CustomerAddress.DOC, CustomerAddress.DOM, CustomerAddress.IsDeleted, CustomerAddress.IsActive FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id WHERE ([Order].IsPaymentDone = 1) " + where + " ORDER BY ordid DESC";


            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                grdGrn.Caption = dt.Rows.Count + ": Rows Found";
                grdGrn.DataSource = dt;
                grdGrn.DataBind();
            }
            else
            {
                grdGrn.Caption = "No Data Found";
                grdGrn.DataSource = null;
                grdGrn.DataBind();
            }

        }
        catch (Exception E)
        {
            ltrErr.Text = E.Message;
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string fdate = txtDate.Text;
        //    string[] fdt = fdate.Split('-');
        //    int date = int.Parse(fdt[0].ToString());
        //    int month = int.Parse(fdt[1].ToString());
        //    int yrar = int.Parse(fdt[2].ToString());
        //    DateTime frmdt = new DateTime(yrar, month, date);

        //    string tdate = txtDate1.Text;
        //    string[] tdt = tdate.Split('-');
        //    int tdate1 = int.Parse(tdt[0].ToString());
        //    int tmonth = int.Parse(tdt[1].ToString());
        //    int tyrar = int.Parse(tdt[2].ToString());
        //    DateTime ftdate = new DateTime(tyrar, tmonth, tdate1);
        //    string qry = "Select  * from [Order] where [Order].createdOnUtc>=CONVERT(varchar,'" + frmdt.ToString("MM-dd-yyyy") + " 00:00:00',204)    and [Order].createdOnUtc<=CONVERT(varchar,'" + ftdate.ToString("MM-dd-yyyy") + " 23:59:59',204)";
        //    DataTable dt = dbc.GetDataTable(qry);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        ltrReqVsGrn.Text = dt.Rows.Count.ToString();
        //    }
        //    gridbind();
        //}
        //catch (Exception ex)
        //{

        //}
    }
}