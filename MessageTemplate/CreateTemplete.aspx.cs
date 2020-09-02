using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class MessageTemplate_CreateTemplete : System.Web.UI.Page
{
    dbConnection dbCon = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindEmailAccount();
            bindToken();
        }
        try
        {
            if (Request.QueryString["Id"] != "" && Request.QueryString["Id"] != null)
            {
                BindData(Request.QueryString["Id"].ToString());
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void BindData(string TempleteId)
    {
        DataTable dt = new DataTable();
        dt = dbCon.GetDataTable("Select * from [MessageTemplate] Where Id=" + TempleteId);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblTempleteID.Text = dt.Rows[0]["Id"].ToString();
            txtname.Text = dt.Rows[0]["Name"].ToString();
            chkisActive.Checked = dt.Rows[0]["IsActive"].ToString().Equals("True") ? true : false;
            txtSubject.Text = dt.Rows[0]["Subject"].ToString();
            txtFullDescription.Text = dt.Rows[0]["Body"].ToString();
            cmbMailAccount.Value = dt.Rows[0]["EmailAccountId"].ToString();
        }
    }
    public void bindEmailAccount()
    {
        DataTable dt = new DataTable();
        dt = dbCon.GetDataTable("Select Id,Email from [EmailAccount] order by Id");
        cmbMailAccount.DataSource = dt;
        cmbMailAccount.DataTextField = "Email";
        cmbMailAccount.DataValueField = "Id";
        cmbMailAccount.DataBind();
    }


    public void bindToken()
    {
        string Token = @"%Order.OrderNumber%, %Order.CustomerFullName%, %Order.CustomerEmail%, %Order.ShippingMethod%, %Order.ShippingPhoneNumber%, %Order.ShippingEmail%, %Order.ShippingAddress1%, %Order.ShippingCity%, %Order.ShippingStateProvince%, %Order.ShippingZipPostalCode%, %Order.ShippingCountry%, %Order.PaymentMethod%, %Order.Product(s)%, %Order.CreatedOn%, %Order.OrderURLForCustomer%, %Shipment.ShipmentNumber%, %Shipment.TrackingNumber%, %Shipment.Product(s)%, %Shipment.URLForCustomer%, %ReturnRequest.Product.Quantity%, %ReturnRequest.Product.Name%, %Customer.Email%, %Customer.Username%, %Customer.FullName%, %Customer.FirstName%, %Customer.LastName%, %Product.ID%, %Product.Name%, %Product.ShortDescription%, %Order.OrderDate%";
        lblTokenlist.Text = Token;
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Id"] != "" && Request.QueryString["Id"] != null)
            {
                if (!lblTempleteID.Text.Equals(""))
                {
                    try
                    {
                        string[] param = { txtname.Text, txtSubject.Text, txtFullDescription.Text, chkisActive.Checked ? "1" : "0", "0", cmbMailAccount.Value, "0", "1", lblTempleteID.Text };
                        string updateTemplete = @"UPDATE  [MessageTemplate] SET [Name] = @1 ,[Subject] = @2 ,[Body] = @3   ,[IsActive] =@4 ,[AttachedDownloadId] = @5 ,[EmailAccountId] =@6 ,[LimitedToStores] = @7,[Priority] = @8 WHERE Id=@9";
                        dbCon.ExecuteQueryWithParams(updateTemplete, param);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            else
            {

                try
                {
                    string[] param = { txtname.Text, txtSubject.Text, txtFullDescription.Text, chkisActive.Checked ? "1" : "0", "0", cmbMailAccount.Value, "0", "1" };
                    string insertTemplet = @"INSERT INTO  [MessageTemplate] ([Name],[Subject],[Body],[IsActive],[AttachedDownloadId],[EmailAccountId],[LimitedToStores],[Priority]) VALUES(@1,@2,@3,@4,@5,@6,@7,@8)";
                   int v= dbCon.ExecuteQueryWithParams(insertTemplet, param);
                }
                catch (Exception)
                {

                }
            }
        }
        catch (Exception)
        {

        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
}