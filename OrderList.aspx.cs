using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class OrderList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    public static string IsAdmin = "", sJurisdictionId = "", IsUserType = "", loginuserId = "";
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
            IsUserType = Request.Cookies["TUser"]["UserType"].ToString();
            loginuserId = Request.Cookies["TUser"]["Id"];
            fillData();
            if(IsUserType == "3")
                grdGrn.Columns[11].Visible = false;
                grdGrn.Columns[12].Visible = false;

            if(IsUserType == "2")
                grdGrn.Columns[12].Visible = false;


            grdGrn.Columns[12].Visible = true;
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
        string qry = string.Empty;
        if (IsUserType == "3")
        {
            int userId = Convert.ToInt32(Request.Cookies["TUser"]["DeliveryId"]);
            string areaQry = " SELECT * from DeliveryDetail where DeliveryID = " + userId;
            DataTable dtArea = dbc.GetDataTable(areaQry);
            if (dtArea.Rows.Count > 0)
            {
                int iCtr = 0;
                string areaIds = string.Empty;
                string buildingIds = string.Empty;
                foreach (DataRow item in dtArea.Rows)
                {
                    if(iCtr > 0)
                    {
                        areaIds += ",";
                        buildingIds += ",";
                    }
                    ++iCtr;
                    areaIds += item.ItemArray[4];
                    buildingIds += item.ItemArray[3];
                }
                qry = "SELECT  Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, " +
                     " isnull(Customer.FirstName,(Select CustomerAddress.FirstName from CustomerAddress " +
                     " where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, " +
                     " (isnull(CustomerAddress.BuildingNo,'') + ' ' + isnull(bm.Building,'') + ' ' + isnull(zm.Area,'') + ' ' + isnull(CustomerAddress.LandMark,'') + ' ' + isnull(CustomerAddress.OtherDetail,isnull(CustomerAddress.Address,''))  + ' ' + isnull(cm.CityName,'') + ' ' + isnull(convert(varchar(20),zm.zipcode),'') + ' ' + isnull(sm.StateName,'')) as cadd, [Order].OrderStatusId, Convert(numeric(18,2),(OrderItem.Quantity * OrderItem.TotalAmount)) as  PaymentAmt, " +
                     " Convert(numeric(18,2),(OrderItem.Quantity * OrderItem.TotalAmount)) AS Totalamt, OrderItem.Quantity as TotalQTY, [Order].BuyWith, " +
                     " [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, Product.Name, " +
                     " OrderStatus.Name AS Ex, AH.ReceiveAmount AS AdminAmount, FH.ReceiveAmount AS FrenchiessAmt, DH.ReceiveAmount AS DeliveryManAmt  " +
                     " FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                     " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                     " INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                     " INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                     " INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                     " LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                     " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') " +
                     "  AND CustomerAddress.AreaId in("+areaIds+ ") AND CustomerAddress.BuildingId in ("+ buildingIds + ")" +
                    " ORDER BY ordid DESC";
            }
        }
        else
        {
            qry = "SELECT  Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, " +
                     " isnull(Customer.FirstName,(Select CustomerAddress.FirstName from CustomerAddress " +
                     " where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, " +
                     " (isnull(CustomerAddress.BuildingNo,'') + ' ' + isnull(bm.Building,'') + ' ' + isnull(zm.Area,'') + ' ' + isnull(CustomerAddress.LandMark,'') + ' ' + isnull(CustomerAddress.OtherDetail,isnull(CustomerAddress.Address,''))  + ' ' + isnull(cm.CityName,'') + ' ' + isnull(convert(varchar(20),zm.zipcode),'') + ' ' + isnull(sm.StateName,'')) as cadd, [Order].OrderStatusId, Convert(numeric(18,2),(OrderItem.Quantity * OrderItem.TotalAmount)) as  PaymentAmt, " +
                     " Convert(numeric(18,2),(OrderItem.Quantity * OrderItem.TotalAmount)) AS Totalamt, OrderItem.Quantity as TotalQTY, [Order].BuyWith, " +
                     " [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, Product.Name, " +
                     " OrderStatus.Name AS Ex, AH.ReceiveAmount AS AdminAmount, FH.ReceiveAmount AS FrenchiessAmt, DH.ReceiveAmount AS DeliveryManAmt " +
                     " FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                     " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                     " INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                     " INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                     " INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                     " LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                     " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') ";
            if (IsUserType == "2")
                qry += " and ISNULL([Order].JurisdictionID,0) =" + sJurisdictionId;
            qry += " ORDER BY ordid DESC";

        }



        //<input type="button" id='del-<%# Eval("ordid")%>' class="btn btn-danger btn-sm" onclick='SubmitData(<%# Eval("ordid")%>)' value="Delivered" />
        DataTable dt = dbc.GetDataTable(qry);
        if (dt != null && dt.Rows.Count > 0)
        {
            // ltrReqVsGrn.Text = dt.Rows.Count.ToString();
            grdGrn.DataSource = dt;
            grdGrn.DataBind();
        }
        IsUserType = Request.Cookies["TUser"]["UserType"].ToString();
        for (int i = 0; i < grdGrn.Rows.Count; i++)
        {
            HiddenField hdn = (HiddenField)grdGrn.Rows[i].FindControl("hdn");
            HiddenField oid = (HiddenField)grdGrn.Rows[i].FindControl("oid");

            HiddenField hdn1 = (HiddenField)grdGrn.Rows[i].FindControl("hdn1");
            HiddenField oid1 = (HiddenField)grdGrn.Rows[i].FindControl("oid1");

            HiddenField hdn2 = (HiddenField)grdGrn.Rows[i].FindControl("hdn2");
            HiddenField oid2 = (HiddenField)grdGrn.Rows[i].FindControl("oid2");

            Literal ltr = (Literal)grdGrn.Rows[i].FindControl("ltr");
            Literal ltr1 = (Literal)grdGrn.Rows[i].FindControl("ltr1");
            Literal ltr2 = (Literal)grdGrn.Rows[i].FindControl("ltr2");
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
            //if (hdn2.Value == "10")
            //{
                DataTable dtchkExists = dbc.GetDataTable("SELECT TOP 1 ReceiveAmount FROM tblPaymentHistory Where OrderId=" + oid2.Value + " AND UserType = " + 3 + " Order By 1 DESC");
            DataTable dtchkjuridictiondataExists = dbc.GetDataTable("SELECT TOP 1 ReceiveAmount FROM tblPaymentHistory Where OrderId=" + oid2.Value + " AND UserType = " + 2 + " Order By 1 DESC");

            if (IsUserType == "3")
                {
                    if (dtchkExists.Rows.Count > 0)
                    {
                        ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm'  value='Payment Received'  disabled />";
                    }
                    else
                    {
                        ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm' onclick='StatusUpdateModal(" + oid2.Value + ")' value='Payment Received' />";
                    }
                    
                }
                else if(IsUserType == "2")
                {
                    if (dtchkExists.Rows.Count > 0)
                    {
                    DataTable dtjuridictiondataExists = dbc.GetDataTable("SELECT TOP 1 ReceiveAmount FROM tblPaymentHistory Where OrderId=" + oid2.Value + " AND UserType = " + IsUserType + " Order By 1 DESC");
                    if (dtjuridictiondataExists.Rows.Count > 0)
                        {
                            ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm'  value='Payment Received'  disabled />";
                        }
                        else
                        {
                            ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm' onclick='StatusUpdateModal(" + oid2.Value + ")' value='Payment Received' />";
                        }
                    }
                    else
                    {
                        ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm'  value='Payment Received'  disabled />";
                    }
                        
                }
                else if(IsUserType == "1")
                {
                if (dtchkExists.Rows.Count > 0 && dtchkjuridictiondataExists.Rows.Count > 0)
                {
                    DataTable dtchkAdmindataExists = dbc.GetDataTable("SELECT TOP 1 ReceiveAmount FROM tblPaymentHistory Where OrderId=" + oid2.Value + " AND UserType = " + IsUserType + " Order By 1 DESC");
                    if (dtchkAdmindataExists.Rows.Count == 0)
                    {
                        ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm' onclick='StatusUpdateModal(" + oid2.Value + ")' value='Payment Received' />";
                    }
                    else
                    {
                        ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm'  value='Payment Received'  disabled />";
                    }
                }
                else
                {
                    ltr2.Text = "<input type='button' id='can-" + oid2.Value + "' class='btn btn-success btn-sm'  value='Payment Received'  disabled />";
                }
            }
            //}

            //else
            //{
            //    ltr2.Text = "";

            //}
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

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static string SavePaymentStatusHistory(string OrderId,string ReceiveAmount)
    {
        dbConnection dbc1 = new dbConnection();
        int IsUserId = Convert.ToInt32(loginuserId);
        int statusId = 40;//Payment Received-Sosho
        if (IsUserType == "3")
        {
            statusId = 20;//Payment Received-Delivery
        }
        else if(IsUserType == "2")
        {
            statusId = 30;//Payment Received-Frenchies
        }
        
        DateTime dt = DateTime.Now;
        DataTable dtchkExists = dbc1.GetDataTable("SELECT Top 1 PHistoryid FROM  [dbo].[tblPaymentHistory] WHERE orderId = " + OrderId + " AND userid = " + IsUserId + " AND UserType = " + IsUserType + " Order by PHistoryid desc");
        if (dtchkExists.Rows.Count > 0)
        {
            string historyId =  dtchkExists.Rows[0]["PHistoryid"].ToString();
            string updateQry = " UPDATE tblPaymentHistory SET OrderId = " + OrderId + " , ReceiveAmount = " + ReceiveAmount + " UserId = " + IsUserId +
                               " , UserType = " + IsUserType + ", ReceiveDate = '" + dt.ToString() + "',StatusId = "+statusId+",ModifiedOn = '" + dt.ToString() + "',ModifiedBy = " + IsUserId +
                               " Where PHistoryId = " + historyId;
            dbc1.ExecuteQuery(updateQry);
        }
        else
        {
            string query = "INSERT INTO [dbo].[tblPaymentHistory] ([OrderId] ,[ReceiveAmount],[UserId],[UserType],[ReceiveDate],[StatusId],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy]) " +
                           " VALUES ('" + OrderId + "','" + ReceiveAmount + "','" + IsUserId + "','" + IsUserType + "','" + dt.ToString() + "',"+statusId+",1,0,'" + dt.ToString() + "'," + IsUserId + ")";
            int VAL = dbc1.ExecuteQuery(query);
        }
        return "1";
    }
}