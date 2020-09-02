using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Shipping_Shipping : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string dtStart = dbc.getindiantime().ToString("dd/MMM/yyyy");
            string dtEnd = dbc.getindiantime().ToString("dd/MMM/yyyy");

            startdate.Value = dtStart;
            enddate.Value = dtEnd;
            string qry = "SELECT Concat(CustomerAddress.FirstName,' ', CustomerAddress.LastName) as cNamae ,CustomerAddress.Address AS cadd, Customer.Mobile, t1.Id AS ordid,(select count(t2.Id) from [Order] as t2 where t2.RefferedOfferCode=t1.CustOfferCode )as totoalrefer, t1.AddressId,  t1.OrderStatusId, t1.OrderTotal AS PaymentAmt, t1.OrderMRP AS Totalamt, t1.TotalQTY, t1.BuyWith, t1.TotalGram, t1.CustReedeemAmount, t1.PaymentGatewayId, Product.Name,  CustomerAddress.Email FROM  [Order] as t1 INNER JOIN Customer ON Customer.Id = t1.CustomerId INNER JOIN CustomerAddress ON t1.AddressId = CustomerAddress.Id INNER JOIN OrderItem ON t1.Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id WHERE (t1.IsPaymentDone = 1) and t1.CustOfferCode!='0' and t1.CustOfferCode!=''    ORDER BY ordid DESC";
          //  string qry = "SELECT Concat(CustomerAddress.FirstName,' ', CustomerAddress.LastName) as cNamae ,CustomerAddress.Address AS cadd, Customer.Mobile, t1.Id AS ordid,(select count(t2.Id) from [Order]as t2 where t2.CustOfferCode=t1.RefferedOfferCode AND t1.RefferedOfferCode IS NOT  NULL AND t1.CustOfferCode IS NOT NULL AND  RefferedOfferCode!='' )as totoalrefer, t1.AddressId,  t1.OrderStatusId, t1.OrderTotal AS PaymentAmt, t1.OrderMRP AS Totalamt, t1.TotalQTY, t1.BuyWith, t1.TotalGram, t1.CustReedeemAmount, t1.PaymentGatewayId, Product.Name,  CustomerAddress.Email FROM  [Order] as t1 INNER JOIN Customer ON Customer.Id = t1.CustomerId INNER JOIN CustomerAddress ON t1.AddressId = CustomerAddress.Id INNER JOIN OrderItem ON t1.Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id WHERE (t1.IsPaymentDone = 1) ORDER BY ordid DESC";

            DataTable dt = dbc.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                // ltrReqVsGrn.Text = dt.Rows.Count.ToString();
                grdGrn.DataSource = dt;
                grdGrn.DataBind();
            }
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            grdGrn.DataSource = null;
            grdGrn.DataBind();


            string dtStart = startdate.Value;
            string dtEnd = enddate.Value;

            if (dtStart != null && dtStart != "" && dtEnd != null && dtEnd != "")
            {


                string qry = "SELECT Concat(CustomerAddress.FirstName,' ', CustomerAddress.LastName) as cNamae ,CustomerAddress.Address AS cadd, Customer.Mobile, t1.Id AS ordid,(select count(t2.Id) from [Order] as t2 where t2.RefferedOfferCode=t1.CustOfferCode )as totoalrefer, t1.AddressId,  t1.OrderStatusId, t1.OrderTotal AS PaymentAmt, t1.OrderMRP AS Totalamt, t1.TotalQTY, t1.BuyWith, t1.TotalGram, t1.CustReedeemAmount, t1.PaymentGatewayId, Product.Name,  CustomerAddress.Email FROM  [Order] as t1 INNER JOIN Customer ON Customer.Id = t1.CustomerId INNER JOIN CustomerAddress ON t1.AddressId = CustomerAddress.Id INNER JOIN OrderItem ON t1.Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id WHERE (t1.IsPaymentDone = 1) and t1.CustOfferCode!='0' and t1.CustOfferCode!='' AND t1.CreatedOnUtc>=CONVERT(varchar,'" + dtStart + " 00:00' , 106) AND t1.CreatedOnUtc<=CONVERT(varchar,'" + dtEnd + " 23:59' , 106)  ORDER BY ordid DESC ";
              //  string qry = "SELECT Concat(CustomerAddress.FirstName,' ', CustomerAddress.LastName) as cNamae ,CustomerAddress.Address AS cadd, Customer.Mobile, t1.Id AS ordid,(select count(t2.Id) from [Order]as t2 where t2.CustOfferCode=t1.RefferedOfferCode AND t1.RefferedOfferCode IS NOT  NULL AND t1.CustOfferCode IS NOT NULL AND  RefferedOfferCode!='' )as totoalrefer, t1.AddressId,  t1.OrderStatusId, t1.OrderTotal AS PaymentAmt, t1.OrderMRP AS Totalamt, t1.TotalQTY, t1.BuyWith, t1.TotalGram, t1.CustReedeemAmount, t1.PaymentGatewayId, Product.Name,  CustomerAddress.Email FROM  [Order] as t1 INNER JOIN Customer ON Customer.Id = t1.CustomerId INNER JOIN CustomerAddress ON t1.AddressId = CustomerAddress.Id INNER JOIN OrderItem ON t1.Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id WHERE (t1.IsPaymentDone = 1) AND t1.CreatedOnUtc>=CONVERT(varchar,'" + dtStart + " 00:00' , 106) AND t1.CreatedOnUtc<=CONVERT(varchar,'" + dtEnd + " 23:59' , 106)  ORDER BY ordid DESC  ";

                DataTable dt = dbc.GetDataTable(qry);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // ltrReqVsGrn.Text = dt.Rows.Count.ToString();

                    grdGrn.DataSource = dt;
                    grdGrn.DataBind();
                }
            }

        }
        catch (Exception)
        {


        }
    }
    protected void grdGrn_RowDataBound(object sender, GridViewRowEventArgs e)
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