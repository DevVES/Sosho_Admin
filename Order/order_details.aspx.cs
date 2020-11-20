using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class order_details : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if(!string.IsNullOrWhiteSpace(Request.QueryString["Orderid"]))
                {
                    string Orderid123 = Request.QueryString["Orderid"].ToString();
                    //string orderrdata = clsCommon.Base64Decode(Orderid123);
                    int oid = 0;
                    int.TryParse(Orderid123.ToString(), out oid);
                    if(oid>0)
                    {
                        
                        DataTable offercode = dbc.GetDataTable("select [Order].CustOfferCode from [Order] where [Order].Id=" + oid);
                        string occ = offercode.Rows[0]["CustOfferCode"].ToString();

                        string qry = "SELECT [Order].Id,Convert(varchar(17),[order].CreatedOnUtc,113) as CreatedOnUtc, [Order].RefferedOfferCode ,Customer.FirstName,  Customer.Mobile,OrderStatus.Name AS OrderStatus FROM [Order] INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id  INNER JOIN Customer ON [Order].CustomerId = Customer.Id where len([Order].RefferedOfferCode)>2 and [Order].RefferedOfferCode='" + occ + "' ";
                        DataTable dtfor = dbc.GetDataTable(qry);

                        if(dtfor!= null && dtfor.Rows.Count>0)
                        {
                            //grd.Caption = "Customer Reffere List";
                            //grd.DataSource = dtfor;
                            //grd.DataBind();
                        }

                        //string addressstr = "select FirstName+' ' +LastName as CustName,Address,(select CityName from CityMaster where CityMaster.Id=CustomerAddress.CityId)as CityName,CustomerAddress.pincode,(select StateMaster.StateName from StateMaster where StateMaster.Id=CustomerAddress.StateId) as StateName,(select CountryMaster.CountryName from CountryMaster where CountryMaster.Id=CustomerAddress.CountryId)as CountryName,CustomerAddress.MobileNo from CustomerAddress where Id=(select AddressId from [Order] where id="+oid+") ";
                        string addressstr = "select " +
                                            " FirstName + ' ' + LastName as CustName, " +
                                            " isnull(ca.buildingno, '') + ' ' + (case isnull(bm.Building, '') when '' then '' else bm.Building + ', ' end) + isnull(zp.Area, '') + ' ' + (case isnull(ca.Landmark,'') when '' then '' else ' Landmark: ' + ca.Landmark end) as [Address], " +
                                            " (select CityName from CityMaster where CityMaster.Id = ca.CityId) as CityName, " +
                                            " ca.pincode, " +
                                            " (select StateMaster.StateName from StateMaster where StateMaster.Id = ca.StateId) as StateName, " +
                                            " (select CountryMaster.CountryName from CountryMaster where CountryMaster.Id = ca.CountryId) as CountryName, " +
                                            " ca.MobileNo " +
                                            " from CustomerAddress ca " +
                                            " left outer join Zipcode zp on zp.id = ca.AreaId " +
                                            " left outer join tblBuilding bm on bm.id = ca.BuildingId " +
                                            " where ca.Id = (select AddressId from[Order] where id = " + oid + ") ";
                        DataTable dtdataa = dbc.GetDataTable(addressstr);

                        if(dtdataa !=null && dtdataa.Rows.Count>0)
                        {
                            lbladdname.InnerHtml = dtdataa.Rows[0]["CustName"].ToString();
                            lbladd.InnerHtml = dtdataa.Rows[0]["Address"].ToString() +  (dtdataa.Rows[0]["Address"].ToString().Trim()==""?"":", ") + dtdataa.Rows[0]["CityName"] + "-" + dtdataa.Rows[0]["pincode"].ToString();
                            lbladdstate.InnerHtml=dtdataa.Rows[0]["StateName"]+", "+dtdataa.Rows[0]["CountryName"];
                            lbladdmob.InnerHtml = dtdataa.Rows[0]["MobileNo"].ToString();
                         //   lblstatus.InnerHtml = dtdataa.Rows[0]["Ex"].ToString();
                           // lblstatus.InnerHtml = dtdataa.Rows[0]["Ex"].ToString();
                            
                        }

                        string OrderDetails = "select [Order].Id as oid,  (DATENAME(dw,CAST(DATEPART(m, CreatedOnUtc) AS VARCHAR)+ '/'+ CAST(DATEPART(d, CreatedOnUtc) AS VARCHAR)  + '/' + CAST(DATEPART(yy, CreatedOnUtc) AS VARCHAR))) +' '+convert(varchar(12),CreatedOnUtc,106)+', '+convert(varchar(12),CreatedOnUtc,108) as EndDate,OrderStatus.Name AS Ex, ISNULL([Order].OrderMRP,0) as MRP,ISNULL([Order].PaidAmount,0)as OrderTotal,[Order].TotalQTY,isnull((select top 1 Name from Payment_Methods where [Order].PaymentGatewayId=Payment_Methods.Id),'Default') as GatwayType  from [Order] INNER JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.Id where [Order].Id=" + oid + "";

                        DataTable dtorderdetails = dbc.GetDataTable(OrderDetails);

                        if(dtorderdetails!=null && dtorderdetails.Rows.Count>0)
                        {
                            lblorderid.InnerHtml = "&nbsp" + dtorderdetails.Rows[0]["oid"].ToString();
                            orderdatedid.InnerHtml = "&nbsp" + dtorderdetails.Rows[0]["EndDate"].ToString();
                            //lblmrp.InnerHtml = "&nbsp" + dtorderdetails.Rows[0]["MRP"].ToString();
                            lbltotordeamt.InnerHtml = "&nbsp" + dtorderdetails.Rows[0]["OrderTotal"].ToString();
                            lblstatus.InnerHtml = "&nbsp" + dtorderdetails.Rows[0]["Ex"].ToString();

                            //lblqtyno.InnerHtml = dtorderdetails.Rows[0]["TotalQTY"].ToString();
                            //lblpayment.InnerHtml = dtorderdetails.Rows[0]["GatwayType"].ToString();

                        }

                        string imaagedetails="select Product.id as pid,product.Name,isnull(Unit,'0') as unitweg,isnull((select UnitName from UnitMaster where UnitMaster.Id=Product.UnitId),'Gram')as Unit from Product where Product.Id=(select ProductId from orderitem where orderid="+oid+")";                        
                        DataTable dtimgstr = dbc.GetDataTable(imaagedetails);
                        

                        if(dtimgstr.Rows.Count>0)
                        {
                            //lblnamee.InnerHtml = dtimgstr.Rows[0]["Name"].ToString();
                            //lblweigh.InnerHtml = dtimgstr.Rows[0]["unitweg"].ToString() + " " + dtimgstr.Rows[0]["Unit"];



                            string productidd = dtimgstr.Rows[0]["pid"].ToString(); 
                            string imagename = "select ImageFileName from ProductImages where Productid="+productidd+"";
                            DataTable dtimage = dbc.GetDataTable(imagename);
                            string query = "SELECT [KeyValue] FROM [StringResources] where KeyName='ProductImageUrl'";
                            DataTable dtfolder = dbc.GetDataTable(query);
                            string folder = "";
                            if (dtfolder.Rows.Count > 0)
                            {
                                folder = dtfolder.Rows[0]["KeyValue"].ToString();
                            }
                            string imgname = folder + dtimage.Rows[0]["ImageFileName"].ToString();




                           // lblimg123.InnerHtml = "<img src=" + imgname + ">";
                           // 
                        }

                      
                       
                    }
                }
               
            }
        }
        catch (Exception ee)
        {
        }
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