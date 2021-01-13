using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class FranchiseeOrderList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    public static string IsAdmin = "", sJurisdictionId = "", IsUserType = "", loginuserId = "", sFranchiseeId = "";
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
            sFranchiseeId = Request.Cookies["TUser"]["FranchiseeId"].ToString();
            fillData();

        }
    }

    public void fillData()
    {
        string qry = string.Empty;
        string franchiseeQry = string.Empty;
        if (IsUserType == "4")
        {
            franchiseeQry = "SELECT C.Id " +
                    " FROM [customer_franchise_link] FL " +
                    " INNER JOIN Franchisee F ON F.FranchiseeCustomerCode = FL.fcode" +
                    " INNER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    " WHERE F.FranchiseeId = " + sFranchiseeId;
        }
        else if (IsUserType == "5")
        {
            franchiseeQry = "SELECT C.Id " +
                    " FROM [customer_franchise_link] FL " +
                    " LEFT OUTER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    "   AND (FL.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.MasterFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR FL.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.MasterFranchiseeId = " + sFranchiseeId + ")) ";
        }
        else if (IsUserType == "6")
        {
            franchiseeQry = "SELECT C.Id " +
                    " FROM [customer_franchise_link] FL " +
                    " LEFT OUTER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    "   WHERE (FL.fcode in (select S.SuperFranchiseeCustomerCode from SuperFranchisee S where S.SuperFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR FL.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.SuperFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR FL.fcode in (select FL.FranchiseeCustomerCode from Franchisee FL where FL.MasterFranchiseeId IN (select ML.MasterFranchiseeId from MasterFranchisee ML where ML.SuperFranchiseeId = " + sFranchiseeId + ")) " +
                        "        OR FL.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.SuperFranchiseeId = " + sFranchiseeId + ")) ";
        }
        string customerIds = string.Empty;
        DataTable dtFranchisees = dbc.GetDataTable(franchiseeQry);
        if (dtFranchisees.Rows.Count > 0)
        {
            int iCtr = 0;

            foreach (DataRow item in dtFranchisees.Rows)
            {
                if (iCtr > 0)
                {
                    customerIds += ",";
                }
                ++iCtr;
                customerIds += item.ItemArray[0];
            }
        }
        if (IsUserType == "4" || IsUserType == "5" || IsUserType == "6")
        {
            if (IsUserType == "4")
            {
                qry = "SELECT Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, " +
                        " isnull(Customer.FirstName, " +
                        " (Select CustomerAddress.FirstName from CustomerAddress " +
                        " where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, " +
                        " (isnull(CustomerAddress.BuildingNo,'') + ' ' + isnull(bm.Building,'') + ' ' + isnull(zm.Area,'') + ' ' + isnull(CustomerAddress.LandMark,'') + ' ' + isnull(CustomerAddress.OtherDetail,isnull(CustomerAddress.Address,''))  + ' ' + isnull(cm.CityName,'') + ' ' + isnull(convert(varchar(20),zm.zipcode),'') + ' ' + isnull(sm.StateName,'')) as cadd, [Order].OrderStatusId, " +
                        " [Order].PaidAmount as  PaymentAmt, " +
                        " [Order].PaidAmount AS Totalamt, 0 as TotalQTY, [Order].BuyWith, " +
                        " [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, '' as [Name], " +
                        " '' AS Ex, 0 AS AdminAmount, 0 AS FrenchiessAmt, " +
                        " 0 AS DeliveryManAmt, '' AS Unit, fl.fcode as OrderSourceName, " +
                        " CustomerAddress.PinCode, ISNULL(zm.Area,'') AS Area, [Order].PaidAmount, '' as CategoryName, '' as SubCategory, " +
                        " 0 as CategoryWiseSummary, '' as VendorName, F.FranchiseeName " +
                        " FROM [Order] " +
                        " INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                        " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id " +
                        " LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId " +
                        " LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId " +
                        " LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId " +
                        " LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                //" INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                //" INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                //" INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                //" LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN UnitMaster U ON OrderItem.UnitId = U.Id " +
                //" LEFT OUTER JOIN Order_Source OS ON OS.OrderId = [Order].Id " +
                //" INNER JOIN (select Distinct CategoryId, SubCategoryId, ProductId from tblCategoryProductLink mcpl " +
                //"             where CategoryId = (select min(CategoryId) from tblCategoryProductLink tcpl " +
                //"             where tcpl.ProductId = mcpl.ProductId)) cl on cl.productid = orderitem.productid " +
                //" inner join Category c on c.CategoryID = cl.CategoryId " +
                //" inner join tblSubCategory s on s.id = cl.SubCategoryId" +
                " inner join customer_franchise_link fl on fl.mobile = Customer.Mobile" +
                        " Left Outer JOIN Franchisee F ON F.FranchiseeCustomerCode = FL.fcode" +
                        " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') and [Order].OrderStatusId<>90 " +
                        "   AND F.FranchiseeId = " + sFranchiseeId;
                //"  and [Customer].Id in(" + customerIds + ")";

                qry += " ORDER BY ordid DESC";
            }
            else if (IsUserType == "5")
            {
                qry = "SELECT Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, " +
                        " isnull(Customer.FirstName, " +
                        " (Select CustomerAddress.FirstName from CustomerAddress " +
                        " where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, " +
                        " (isnull(CustomerAddress.BuildingNo,'') + ' ' + isnull(bm.Building,'') + ' ' + isnull(zm.Area,'') + ' ' + isnull(CustomerAddress.LandMark,'') + ' ' + isnull(CustomerAddress.OtherDetail,isnull(CustomerAddress.Address,''))  + ' ' + isnull(cm.CityName,'') + ' ' + isnull(convert(varchar(20),zm.zipcode),'') + ' ' + isnull(sm.StateName,'')) as cadd, [Order].OrderStatusId, " +
                        " [Order].PaidAmount as  PaymentAmt, " +
                        " [Order].PaidAmount AS Totalamt, 0 as TotalQTY, [Order].BuyWith, " +
                        " [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, '' as [Name], " +
                        " '' AS Ex, 0 AS AdminAmount, 0 AS FrenchiessAmt, " +
                        " 0 AS DeliveryManAmt, '' AS Unit, fl.fcode as OrderSourceName, " +
                        " CustomerAddress.PinCode, ISNULL(zm.Area,'') AS Area, [Order].PaidAmount, '' as CategoryName, '' as SubCategory, " +
                        " 0 as CategoryWiseSummary, '' as VendorName, " + 
                        " isnull(fs.FranchiseeName,ISNULL(mfs.MasterFranchiseeName,isnull(sfs.SuperFranchiseeName,''))) as FranchiseeName  " +
                        " FROM [Order] " +
                        " INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                        " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id " +
                        " LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId " +
                        " LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId " +
                        " LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId " +
                        " LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                //" INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                //" INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                //" INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                //" LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN UnitMaster U ON OrderItem.UnitId = U.Id " +
                //" LEFT OUTER JOIN Order_Source OS ON OS.OrderId = [Order].Id " +
                //" INNER JOIN (select Distinct CategoryId, SubCategoryId, ProductId from tblCategoryProductLink mcpl " +
                //"             where CategoryId = (select min(CategoryId) from tblCategoryProductLink tcpl " +
                //"             where tcpl.ProductId = mcpl.ProductId)) cl on cl.productid = orderitem.productid " +
                //" inner join Category c on c.CategoryID = cl.CategoryId " +
                //" inner join tblSubCategory s on s.id = cl.SubCategoryId" +
                " inner join customer_franchise_link fl on fl.mobile = Customer.Mobile" +
                " left outer join SuperFranchisee sfs on fl.fcode = sfs.SuperFranchiseeCustomerCode " +
                " left outer join MasterFranchisee mfs on fl.fcode = mfs.MasterFranchiseeCustomerCode " +
                " left outer join Franchisee fs on fl.fcode = fs.FranchiseeCustomerCode " +
                        //" INNER JOIN MasterFranchisee F ON F.MasterFranchiseeCustomerCode = FL.fcode" +
                        " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') and [Order].OrderStatusId<>90 " +
                        "   AND (fl.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.MasterFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR fl.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.MasterFranchiseeId = " + sFranchiseeId + ")) ";
                //"   AND F.FranchiseeId = " + sFranchiseeId;
                //"  and [Customer].Id in(" + customerIds + ")";

                qry += " ORDER BY ordid DESC";
            }
            else if (IsUserType == "6")
            {
                qry = "SELECT Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc ,[Order].Id as ordid, " +
                        " isnull(Customer.FirstName, " +
                        " (Select CustomerAddress.FirstName from CustomerAddress " +
                        " where CustomerAddress.Id= [Order].AddressId)) as FirstName, [Order].AddressId,CustomerAddress.MobileNo  as Mobile, " +
                        " (isnull(CustomerAddress.BuildingNo,'') + ' ' + isnull(bm.Building,'') + ' ' + isnull(zm.Area,'') + ' ' + isnull(CustomerAddress.LandMark,'') + ' ' + isnull(CustomerAddress.OtherDetail,isnull(CustomerAddress.Address,''))  + ' ' + isnull(cm.CityName,'') + ' ' + isnull(convert(varchar(20),zm.zipcode),'') + ' ' + isnull(sm.StateName,'')) as cadd, [Order].OrderStatusId, " +
                        " [Order].PaidAmount as  PaymentAmt, " +
                        " [Order].PaidAmount AS Totalamt, 0 as TotalQTY, [Order].BuyWith, " +
                        " [Order].TotalGram, [Order].CustReedeemAmount, [Order].PaymentGatewayId, '' as [Name], " +
                        " '' AS Ex, 0 AS AdminAmount, 0 AS FrenchiessAmt, " +
                        " 0 AS DeliveryManAmt, '' AS Unit, fl.fcode as OrderSourceName, " +
                        " CustomerAddress.PinCode, ISNULL(zm.Area,'') AS Area, [Order].PaidAmount, '' as CategoryName, '' as SubCategory, " +
                        " 0 as CategoryWiseSummary, '' as VendorName," +
                        " isnull(fs.FranchiseeName,ISNULL(mfs.MasterFranchiseeName,isnull(sfs.SuperFranchiseeName,''))) as FranchiseeName  " +
                        " FROM [Order] " +
                        " INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                        " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id " +
                        " LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId " +
                        " LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId " +
                        " LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId " +
                        " LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                //" INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                //" INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                //" INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                //" LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                //" LEFT OUTER JOIN UnitMaster U ON OrderItem.UnitId = U.Id " +
                //" LEFT OUTER JOIN Order_Source OS ON OS.OrderId = [Order].Id " +
                //" INNER JOIN (select Distinct CategoryId, SubCategoryId, ProductId from tblCategoryProductLink mcpl " +
                //"             where CategoryId = (select min(CategoryId) from tblCategoryProductLink tcpl " +
                //"             where tcpl.ProductId = mcpl.ProductId)) cl on cl.productid = orderitem.productid " +
                //" inner join Category c on c.CategoryID = cl.CategoryId " +
                //" inner join tblSubCategory s on s.id = cl.SubCategoryId" +
                " inner join customer_franchise_link fl on fl.mobile = Customer.Mobile" +
                " left outer join SuperFranchisee sfs on fl.fcode = sfs.SuperFranchiseeCustomerCode " +
                " left outer join MasterFranchisee mfs on fl.fcode = mfs.MasterFranchiseeCustomerCode " +
                " left outer join Franchisee fs on fl.fcode = fs.FranchiseeCustomerCode " +
                        //" INNER JOIN MasterFranchisee F ON F.MasterFranchiseeCustomerCode = FL.fcode" +
                        " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') and [Order].OrderStatusId<>90 " +
                        "   AND (fl.fcode in (select S.SuperFranchiseeCustomerCode from SuperFranchisee S where S.SuperFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR fl.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.SuperFranchiseeId = " + sFranchiseeId + ") " +
                        "        OR fl.fcode in (select FL.FranchiseeCustomerCode from Franchisee FL where FL.MasterFranchiseeId IN (select ML.MasterFranchiseeId from MasterFranchisee ML where ML.SuperFranchiseeId = " + sFranchiseeId + ")) " +
                "        OR fl.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.SuperFranchiseeId = " + sFranchiseeId + ")) ";
                //"   AND F.FranchiseeId = " + sFranchiseeId;
                //"  and [Customer].Id in(" + customerIds + ")";

                qry += " ORDER BY ordid DESC";
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
                     " OrderStatus.Name AS Ex, AH.ReceiveAmount AS AdminAmount, FH.ReceiveAmount AS FrenchiessAmt, " +
                     " DH.ReceiveAmount AS DeliveryManAmt, OrderItem.Unit +' '+ U.UnitName AS Unit,OS.OrderSourceName, " +
                     " CustomerAddress.PinCode, ISNULL(zm.Area,'') AS Area, [Order].PaidAmount, c.CategoryName, s.SubCategory, " +
                     " (select sum(Convert(numeric(18, 2), (oii.Quantity * oii.TotalAmount))) from OrderItem oii " +
                     "      inner join (select Distinct CategoryId, SubCategoryId, ProductId from tblCategoryProductLink) icpl on icpl.productid = oii.productid " +
                     "      where icpl.categoryid = cl.CategoryId and oii.OrderId = [order].Id " +
                     " ) CategoryWiseSummary, IsNull(Product.VideoName,'') as VendorName, '' as FranchiseeName  " +
                     " FROM [Order] INNER JOIN Customer ON Customer.Id = [Order].CustomerId " +
                     " INNER JOIN CustomerAddress ON [Order].AddressId = CustomerAddress.Id LEFT OUTER JOIN StateMaster sm on sm.Id = CustomerAddress.StateId LEFT OUTER JOIN CityMaster cm on cm.Id = CustomerAddress.CityId LEFT OUTER JOIN Zipcode zm on zm.Id = CustomerAddress.AreaId LEFT OUTER JOIN tblBuilding bm on bm.Id = CustomerAddress.BuildingId " +
                     " INNER JOIN OrderItem ON [Order].Id = OrderItem.OrderId " +
                     " INNER JOIN Product ON OrderItem.ProductId = Product.Id " +
                     " INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id " +
                     " LEFT OUTER JOIN tblPaymentHistory AH ON AH.UserType = 1 AND AH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory FH ON FH.UserType = 2 AND FH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN tblPaymentHistory DH ON DH.UserType = 3 AND DH.OrderId = [Order].Id " +
                     " LEFT OUTER JOIN UnitMaster U ON OrderItem.UnitId = U.Id " +
                     " LEFT OUTER JOIN Order_Source OS ON OS.OrderId = [Order].Id " +
                     " INNER JOIN (select Distinct CategoryId, SubCategoryId, ProductId from tblCategoryProductLink mcpl " +
                     "             where CategoryId = (select min(CategoryId) from tblCategoryProductLink tcpl " +
                     "             where tcpl.ProductId = mcpl.ProductId)) cl on cl.productid = orderitem.productid " +
                     " inner join Category c on c.CategoryID = cl.CategoryId " +
                     " inner join tblSubCategory s on s.id = cl.SubCategoryId" +
                     " WHERE ([Order].CreatedOnUtc>='" + startdate.Value + " 00:00:00') AND ([Order].CreatedOnUtc<='" + enddate.Value + " 23:59:59') and [Order].OrderStatusId<>90 ";
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