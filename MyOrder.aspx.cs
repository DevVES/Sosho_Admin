using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyOrder : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string CustomerId = clsCommon.getCurrentCustomer().id;
                if (!string.IsNullOrWhiteSpace(CustomerId))
                {
                    string query = "SELECT [KeyValue] FROM [StringResources] where KeyName='ProductImageUrl'";
                    DataTable dtfolder = dbc.GetDataTable(query);
                    string folder = "";
                    if (dtfolder.Rows.Count > 0)
                    {
                        folder = dtfolder.Rows[0]["KeyValue"].ToString();
                    }
                    Session["OrderId"] = "";
                    string Querydata = "select [Order].CustOfferCode as cccode,[Order].RefferedOfferCode as Refercode,(select top 1 Product.EndDate from Product where Product.Id=OrderItem.ProductId order by Product.Id desc) as enddatetime,(select top 1 Product.Id from Product where Product.Id=OrderItem.ProductId order by Product.Id desc) as productid,[Order].id as OrderId,(select top 1 Name from Product where Product.Id=OrderItem.ProductId order by Product.Id desc) as ProductName,(select  (DATENAME(dw,CAST(DATEPART(m, EndDate) AS VARCHAR)+ '/'+ CAST(DATEPART(d, EndDate) AS VARCHAR)  + '/' + CAST(DATEPART(yy, EndDate) AS VARCHAR))) +' '+convert(varchar(12),EndDate,106)+', '+convert(varchar(12),EndDate,108) as EndDate from  Product where Product.Id=OrderItem.ProductId and Product.EndDate is not null) as EndDate,[Order].OrderTotal from [Order] inner join OrderItem on OrderItem.OrderId=[Order].Id where [Order].CustomerId=" + CustomerId + " order by [Order].id desc";

                    DataTable dtproduct = dbc.GetDataTable(Querydata);

                    string ProductNameinmsg = "";
                    bool ismrp = false;
                    string Orderdetails = "";

                    if (dtproduct != null && dtproduct.Rows.Count > 0)
                    {
                        string dtdata = dbc.getindiantime().ToString("dd/MMM/yyyy HH:mm:ss tt");
                        DataTable dtlive = new DataTable();
                        try
                        {
                            dtlive = dtproduct.Select("enddatetime<='" + dbc.getindiantime().ToString("dd/MMM/yyyy HH:mm:ss tt") + "'").CopyToDataTable();
                        }
                        catch (Exception)
                        {


                        }

                        string mrp = "";
                        string forcust = "";
                        string date = "";
                        string custid = clsCommon.getCurrentCustomer().id;
                        clsModals.custorderdetails datacust = clsCommon.custdetails(custid);
                        for (int i = 0; dtproduct.Rows.Count > i; i++)
                        {


                            string productidd = dtproduct.Rows[i]["productid"].ToString();
                            string orderidd = dtproduct.Rows[i]["OrderId"].ToString();
                            string imagename = "select ImageFileName from ProductImages where Productid=" + productidd + "";
                            DataTable dtimage = dbc.GetDataTable(imagename);
                            string imgname = "";
                            if (dtimage != null && dtimage.Rows.Count > 0)
                            {
                                imgname = folder + dtimage.Rows[0]["ImageFileName"].ToString();
                            }

                            int flag1 = 0;

                            if (dtlive != null && dtlive.Rows.Count > 0)
                            {
                                DataRow[] drr = dtlive.Select("OrderId=" + orderidd);

                                if (drr.Length > 0)
                                {
                                    flag1 = 1;
                                }
                            }

                            string qry = "select Product.Id as ProductId,Product.ProductMrp,Product.Name,Product.[DisplayNameInMsg],Product.[ShowMrpInMsg] , Product.Mrp, Product.BuyWith1FriendExtraDiscount, Product.BuyWith5FriendExtraDiscount, (CONVERT(varchar ,Product.EndDate,106)) +' ' + (CONVERT(varchar ,Product.EndDate,108)) as pedate, [Order].Id as orderid, OrderItem.Id as orderitemid  from Product Inner join OrderItem on OrderItem.ProductId=Product.Id Inner Join [Order] On [Order].Id = OrderItem.OrderId where [Order].Id=" + orderidd;
                            DataTable dt = dbc.GetDataTable(qry);


                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string flg = datacust.Buywith;
                                ProductNameinmsg = dt.Rows[0]["DisplayNameInMsg"].ToString();

                                if (ProductNameinmsg.Length == 0 || ProductNameinmsg == null || ProductNameinmsg == "")
                                {
                                    ProductNameinmsg = dt.Rows[0]["Name"].ToString();
                                }
                                ismrp = bool.Parse(dt.Rows[0]["ShowMrpInMsg"].ToString());
                                mrp = dt.Rows[0]["Mrp"].ToString();

                                if (flg == "1")
                                {
                                    forcust = dt.Rows[0]["Mrp"].ToString();
                                }
                                else if (flg == "2")
                                {
                                    forcust = dt.Rows[0]["BuyWith1FriendExtraDiscount"].ToString();
                                }
                                else if (flg == "5")
                                {
                                    forcust = dt.Rows[0]["BuyWith5FriendExtraDiscount"].ToString();
                                }
                                date = dt.Rows[0]["pedate"].ToString();

                            }

                            string orderid64 = clsCommon.Base64Encode(dtproduct.Rows[i]["OrderId"].ToString());

                            string productid1 = dtproduct.Rows[i]["productid"].ToString();

                            string getdate = clsCommon.getProductExpiredOnDate(productid1);

                            bool produstatus = clsCommon.ProductStatus(productid1);
                            if (produstatus == false)
                            {

                                Orderdetails += "<div class=\"row\"> <div class=\"order-list\"> <ul> <li> <div class=\"inner\"> <div class=\"orderid left\"> <p><strong>" + dtproduct.Rows[i]["OrderId"].ToString() + "</strong></p> </div> <div class=\"left olist-date\"> <p>" + getdate + "</p> </div> <div class=\"amount right\"> <p>Order Total:<span>" + dtproduct.Rows[i]["OrderTotal"] + "</span></p> </div> </div> </li> </ul> <table class=\"table table-bordered\"> <tbody> <tr> <td> <div class=\"left olist-img\"> <a href=\"\"> <img src=\"" + imgname + "\" /></a> </div> <div class=\"left olist-name\"> <p>" + dtproduct.Rows[i]["ProductName"] + "</p><p class=\"olist-expired\">Expiring on " + getdate + "</p>  </div> <div class=\"right\"> <div class=\"details\"> <a href=\"order_details.aspx?Orderid=" + orderid64 + "\"> <p>View Details</p></a>";
                            }
                            else
                            {
                                Orderdetails += "<div class=\"row\"> <div class=\"order-list\"> <ul> <li> <div class=\"inner\"> <div class=\"orderid left\"> <p><strong>" + dtproduct.Rows[i]["OrderId"].ToString() + "</strong></p> </div> <div class=\"left olist-date\"> <p>" + getdate + "</p> </div> <div class=\"amount right\"> <p>Order Total:<span>" + dtproduct.Rows[i]["OrderTotal"] + "</span></p> </div> </div> </li> </ul> <table class=\"table table-bordered\"> <tbody> <tr> <td> <div class=\"left olist-img\"> <a href=\"\"> <img src=\"" + imgname + "\" /></a> </div> <div class=\"left olist-name\"> <p>" + dtproduct.Rows[i]["ProductName"] + "</p><p class=\"olist-expired\">Expired on " + getdate + "</p>  </div> <div class=\"right\"> <div class=\"details\"> <a href=\"order_details.aspx?Orderid=" + orderid64 + "\"> <p>View Details</p></a>";
                            }
                            string Couponcode = dtproduct.Rows[i]["cccode"].ToString();
                            string link = "";
                            if (Couponcode == "0")
                            {

                            }
                            else
                            {
                                link = "?offercode=" + Couponcode + "";
                            }

                            string mrpd = "";

                            if (ismrp == true)
                            {
                                mrpd = "(MRP ₹ " + dt.Rows[0]["ProductMrp"].ToString() + ")";
                            }
                            if (flag1 < 1)
                            {
                                //Orderdetails+="<p class=\"share\">Share on</p> <div> <a href=\"\"><img src=\"images/facebook.png\"></a> <a href=\"\"><img src=\"images/instagram.png\"></a> <a href=\"\"><img src=\"images/whatsapp.png\"></a> </div>";

                                Orderdetails += "<p class=\"share\">Share on</p> <div class=\"social-links-myorder\"> <a target=\"_blank\" href=\"https://web.whatsapp.com/send?text=" + clsCommon.WhatsappmsgKey("OrderSummaryPageMsg") + "" + link + " \"share/whatsapp/share\" data-action=\"share/whatsapp/share\"><img style=\"margin: 0 6px 0 0;border-radius: 4px;\" id=\"web\" src=\"images/whatsapp.png\" /></a> <a target=\"_blank\" href=\"whatsapp://send?text=" + clsCommon.WhatsappmsgKey("OrderSummaryPageMsg") + "" + link + "\"><img id=\"mo-wa\" src=\"images/whatsapp.png\" /></a> <span id=\"fb-share-button\" style=\"padding-right:6px;\"></span></div>";
                            }

                            //<i class=\"fa fa-facebook\" style=\"cursor:pointer;font-size:18px;background: #3b5998;padding: 3px;margin-right: 0px;margin-top:2px;color: #fff;border-radius: 4px;\"></i>

                            Orderdetails += "</div> </div> </td>  </tr>  </tbody> </table> </div> </div>";




                        }
                    }
                    orderlistDetails.InnerHtml = Orderdetails;

                }
                else
                {

                }
            }
        }
        catch (Exception ee)
        {


        }
    }

}