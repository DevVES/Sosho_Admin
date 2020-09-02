using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class OrderList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string IsAdmin = "", sJurisdictionId = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string dtStart = dbc.getindiantime().ToString("dd/MMM/yyyy");
            string dtEnd = dbc.getindiantime().ToString("dd/MMM/yyyy");
            startdate.Value = dtStart;
            enddate.Value = dtEnd;
            IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
            sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
            fillData();
        }

    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            fillData();
        }
        catch (Exception)
        {

        }
    }
    public void fillData()
    {
        string qry = "SELECT  Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, isnull(Customer.FirstName,(Select CustomerAddress.FirstName from CustomerAddress where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, CustomerAddress.Address AS cadd, [Order].OrderStatusId, OrderItem.Quantity * OrderItem.MrpPerUnit as  PaymentAmt,OrderItem.Quantity * OrderItem.MrpPerUnit AS Totalamt, OrderItem.Quantity as TotalQTY, [Order].BuyWith, [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, Product.Name,OrderStatus.Name AS Ex FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId INNER JOIN Product ON OrderItem.ProductId = Product.Id INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') "; 
        if (IsAdmin == "False")
            qry += " and JurisdictionID=" + sJurisdictionId;
        qry += " ORDER BY ordid DESC";





        //<input type="button" id='del-<%# Eval("ordid")%>' class="btn btn-danger btn-sm" onclick='SubmitData(<%# Eval("ordid")%>)' value="Delivered" />
        DataTable dt = dbc.GetDataTable(qry);
        if (dt != null && dt.Rows.Count > 0)
        {
            // ltrReqVsGrn.Text = dt.Rows.Count.ToString();
            grdGrn.DataSource = dt;
            grdGrn.DataBind();
        }

        for (int i = 0; i < grdGrn.Rows.Count; i++)
        {
            HiddenField hdn = (HiddenField)grdGrn.Rows[i].FindControl("hdn");
            HiddenField oid = (HiddenField)grdGrn.Rows[i].FindControl("oid");

            HiddenField hdn1 = (HiddenField)grdGrn.Rows[i].FindControl("hdn1");
            HiddenField oid1 = (HiddenField)grdGrn.Rows[i].FindControl("oid1");

            Literal ltr = (Literal)grdGrn.Rows[i].FindControl("ltr");
            Literal ltr1 = (Literal)grdGrn.Rows[i].FindControl("ltr1");
            if (hdn.Value == "10")
            {
                ltr.Text = "<input type='button' id='del-" + oid.Value + "' class='btn btn-danger btn-sm' onclick='SubmitData(" + oid.Value + ")' value='Delivered' />";

            }
            else
            {
                ltr.Text = "";

            }

            if (hdn1.Value == "10")
            {
                ltr1.Text = "<input type='button' id='can-" + oid1.Value + "' class='btn btn-success btn-sm' onclick='Cancel(" + oid1.Value + ")' value='Cancel' />";
            }

            else
            {
                ltr1.Text = "";

            }
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

    [WebMethod]
    public static string SaveData(string Id)
    {
        dbConnection dbc1 = new dbConnection();
        try
        {
            if (dbc1.ExecuteQuery("Update [order] set OrderStatusId=70 where id=" + Id) > 0)
            {
                string qry = "SELECT Customer.Mobile, [Order].Id From [Order] INNER JOIN Customer ON [Order].CustomerId = Customer.Id where [Order].Id=" + Id;

                DataTable dt = dbc1.GetDataTable(qry);
                string mono = "";
                string sms = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    string oid = dt.Rows[0]["Id"].ToString();
                    mono = dt.Rows[0]["Mobile"].ToString();
                    sms = "Your SoSho order ID " + oid + " has been successfully delivered. We hope that you like our product. To get more discounted offers, stay updated on SoSho.in";
                }
                dbc1.SendSMS(mono, sms);
            }
            return "1";
        }
        catch (Exception ex)
        {
        }
        return "0";
    }

    [WebMethod]
    public static string SaveData1(string Id)
    {
        dbConnection dbc1 = new dbConnection();
        try
        {
            if (dbc1.ExecuteQuery("Update [order] set OrderStatusId=90 where id=" + Id) > 0)
            {
                string qry = "SELECT Customer.Mobile, [Order].Id From [Order] INNER JOIN Customer ON [Order].CustomerId = Customer.Id where [Order].Id=" + Id;

                DataTable dt = dbc1.GetDataTable(qry);
                string mono = "";
                string sms = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    string oid = dt.Rows[0]["Id"].ToString();
                    mono = dt.Rows[0]["Mobile"].ToString();
                    sms = "Your SoSho order number " + oid + " has been cancelled. We would like to see you around again. Get more discounts and offers only on https://sosho.in.";
                }
                dbc1.SendSMS(mono, sms);
            }
            return "1";
        }
        catch (Exception ex)
        {
        }
        return "0";
    }
}