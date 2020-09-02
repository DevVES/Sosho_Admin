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
            string Orderid123 = "";
            if (!IsPostBack)
            {
                //string data = Session["OrderId"].ToString();

                //starting 20_10_2019
                if (!string.IsNullOrWhiteSpace(Request.QueryString["Orderid"]))
                {
                    Orderid123 = Request.QueryString["Orderid"].ToString();
                }

                if (Session["OrderId"] != null && Session["OrderId"] != "")
                {
                    Orderid123 = Session["OrderId"].ToString();
                }
                string orderrdata = clsCommon.Base64Decode(Orderid123);
                int oid = 0;
                int.TryParse(orderrdata.ToString(), out oid);
                if (oid > 0)
                {

                    //string addressstr = "select FirstName+' ' +LastName as CustName,Address,(select CityName from CityMaster where CityMaster.Id=CustomerAddress.CityId)as CityName,CustomerAddress.pincode,(select StateMaster.StateName from StateMaster where StateMaster.Id=CustomerAddress.StateId) as StateName,(select CountryMaster.CountryName from CountryMaster where CountryMaster.Id=CustomerAddress.CountryId)as CountryName,CustomerAddress.MobileNo from CustomerAddress where Id=(select AddressId from [Order] where id="+oid+") ;";

                    string addressstr = "select FirstName as CustName,Address,(select CityName from CityMaster where CityMaster.Id=CustomerAddress.CityId)as CityName,CustomerAddress.pincode,(select StateMaster.StateName from StateMaster where StateMaster.Id=CustomerAddress.StateId) as StateName,(select CountryMaster.CountryName from CountryMaster where CountryMaster.Id=CustomerAddress.CountryId)as CountryName,CustomerAddress.MobileNo from CustomerAddress where Id=(select AddressId from [Order] where id=" + oid + ") ;";

                    DataTable dtdataa = dbc.GetDataTable(addressstr);




                    if (dtdataa != null && dtdataa.Rows.Count > 0)
                    {
                        lbladdname.InnerHtml = dtdataa.Rows[0]["CustName"].ToString();
                        lbladd.InnerHtml = dtdataa.Rows[0]["Address"].ToString() + ", " + dtdataa.Rows[0]["CityName"] + "-" + dtdataa.Rows[0]["pincode"].ToString();
                        lbladdstate.InnerHtml = dtdataa.Rows[0]["StateName"] + ", " + dtdataa.Rows[0]["CountryName"];
                        lbladdmob.InnerHtml = dtdataa.Rows[0]["MobileNo"].ToString();
                    }

                    string OrderDetails = "select [Order].BuyWith,[Order].CustOfferCode,[Order].Id as oid,  (DATENAME(dw,CAST(DATEPART(m, CreatedOnUtc) AS VARCHAR)+ '/'+ CAST(DATEPART(d, CreatedOnUtc) AS VARCHAR)  + '/' + CAST(DATEPART(yy, CreatedOnUtc) AS VARCHAR))) +' '+convert(varchar(12),CreatedOnUtc,106)+', '+convert(varchar(12),CreatedOnUtc,108) as EndDate,ISNULL([Order].OrderMRP,0) as MRP,ISNULL([Order].OrderTotal,0)as OrderTotal,[Order].TotalQTY,isnull((select top 1 Name from Payment_Methods where [Order].PaymentGatewayId=Payment_Methods.Id),'Default') as GatwayType  from [Order] where [Order].Id=" + oid + "";

                    DataTable dtorderdetails = dbc.GetDataTable(OrderDetails);

                    string imaagedetails = "select Product.Name,Product.Mrp,Product.ProductMrp,Product.[DisplayNameInMsg],Product.[ShowMrpInMsg], Product.BuyWith1FriendExtraDiscount, Product.BuyWith5FriendExtraDiscount, Product.id as pid,product.Name,isnull(Unit,'0') as unitweg,isnull((select UnitName from UnitMaster where UnitMaster.Id=Product.UnitId),'Gram')as Unit from Product where Product.Id=(select ProductId from orderitem where orderid=" + oid + ")";
                    DataTable dtimgstr = dbc.GetDataTable(imaagedetails);

                    string productid = dtimgstr.Rows[0]["pid"].ToString();
                    bool status = clsCommon.ProductStatus(productid);
                    if (status == true)
                    {
                        disp.InnerHtml = "";
                    }
                    string ProductNameinmsg = "";
                    bool ismrp = false;

                    if (dtorderdetails != null && dtorderdetails.Rows.Count > 0)
                    {

                        ProductNameinmsg = dtimgstr.Rows[0]["DisplayNameInMsg"].ToString();
                        ismrp = bool.Parse(dtimgstr.Rows[0]["ShowMrpInMsg"].ToString());

                        lblorderid.InnerHtml = dtorderdetails.Rows[0]["oid"].ToString();
                        orderdatedid.InnerHtml = dtorderdetails.Rows[0]["EndDate"].ToString();
                        lblmrp.InnerHtml = dtorderdetails.Rows[0]["MRP"].ToString();
                        lbltotordeamt.InnerHtml = dtorderdetails.Rows[0]["OrderTotal"].ToString();
                        lblqtyno.InnerHtml = dtorderdetails.Rows[0]["TotalQTY"].ToString();
                        lblpayment.InnerHtml = dtorderdetails.Rows[0]["GatwayType"].ToString();
                        string productname = dtimgstr.Rows[0]["Name"].ToString();

                        if (ProductNameinmsg.Length == 0 || ProductNameinmsg == null || ProductNameinmsg == "")
                        {
                            ProductNameinmsg = dtimgstr.Rows[0]["Name"].ToString();
                        }
                        string productidd = dtimgstr.Rows[0]["pid"].ToString();
                        string flag = dtorderdetails.Rows[0]["BuyWith"].ToString();
                        string custorder = dtorderdetails.Rows[0]["CustOfferCode"].ToString();

                        string mess = "";
                        string forcust = "";
                        string mrp = "";
                        mrp = dtimgstr.Rows[0]["Mrp"].ToString();
                        string date = clsCommon.getProductExpiredOnDate(productidd);
                        string ocode = "";
                        if (custorder != null && custorder != "")
                        {
                            ocode = "/?offercode=" + custorder;
                        }
                        string mrpd = "";

                        if (ismrp == true)
                        {
                            mrpd = "(MRP ₹ " + dtimgstr.Rows[0]["ProductMrp"].ToString() + ")";
                        }
                        if (flag == "1")
                        {
                            forcust = dtimgstr.Rows[0]["Mrp"].ToString();

                            //string title = "Final Step";
                            string[] daaata = forcust.Split('.');
                            forcust = daaata[0].ToString();
                            string[] mrpdata = mrp.Split('.');
                            mrp = mrpdata[0].ToString();
                            mess = "Share this offer on WhatsApp so that your friends can also make the most if it!";
                        }
                        else if (flag == "2")
                        {
                            forcust = dtimgstr.Rows[0]["BuyWith1FriendExtraDiscount"].ToString();

                            //string title = "Final Step";
                            string[] daaata = forcust.Split('.');
                            forcust = daaata[0].ToString();
                            string[] mrpdata = mrp.Split('.');
                            mrp = mrpdata[0].ToString();
                            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
                            mess = "Since you have chosen to buy with 1 friend, share offer to ensure your friend buys it by " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.Link sosho.in";


                        }
                        else if (flag == "6")
                        {
                            forcust = dtimgstr.Rows[0]["BuyWith5FriendExtraDiscount"].ToString();
                            string[] daaata = forcust.Split('.');
                            forcust = daaata[0].ToString();


                            string[] mrpdata = mrp.Split('.');
                            mrp = mrpdata[0].ToString();
                            //string title = "Final Step";

                            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
                            mess = "Since you have chosen to buy with 4 friends, share offer to ensure your friends buy it by  " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.";
                        }

                       // lblmsgtest.InnerHtml = "<span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"web\" href=\"https://web.whatsapp.com/send?text=Hi! I just bought " + ProductNameinmsg + " at just ₹" + forcust + " " + mrpd + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + " \"share/whatsapp/share\" target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span> <span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"mo-wa\" href=\"whatsapp://send?text=Hi! I just bought " + ProductNameinmsg + " at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + " target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span>";
                        lblmsgtest.InnerHtml = "<span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"web\" href=\"https://web.whatsapp.com/send?text=" + clsCommon.WhatsappmsgKey("OrderDetail") + "" + ocode + " \"share/whatsapp/share\" target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span> <span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"mo-wa\" href=\"whatsapp://send?text=" + clsCommon.WhatsappmsgKey("OrderDetail") + "" + ocode + " target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span>";

                    }
                    if (dtimgstr.Rows.Count > 0)
                    {
                        lblnamee.InnerHtml = dtimgstr.Rows[0]["Name"].ToString();
                        lblweigh.InnerHtml = dtimgstr.Rows[0]["unitweg"].ToString() + " " + dtimgstr.Rows[0]["Unit"];



                        string productidd = dtimgstr.Rows[0]["pid"].ToString();
                        string imagename = "select ImageFileName from ProductImages where Productid=" + productidd + "";
                        DataTable dtimage = dbc.GetDataTable(imagename);
                        string query = "SELECT [KeyValue] FROM [StringResources] where KeyName='ProductImageUrl'";
                        DataTable dtfolder = dbc.GetDataTable(query);
                        string folder = "";
                        string imgname = "";
                        if (dtfolder != null && dtfolder.Rows.Count > 0)
                        {
                         
                                folder = dtfolder.Rows[0]["KeyValue"].ToString();

                                if (dtimage != null && dtimage.Rows.Count > 0)
                                {
                                    imgname = folder + dtimage.Rows[0]["ImageFileName"].ToString();
                                }
                                
                         


                        }
                       
                        lblimg123.InnerHtml = "<img src=" + imgname + ">";

                    }


                }
            }

        }
        catch (Exception ee)
        {
        }
    }
}







//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class order_details : System.Web.UI.Page
//{
//    dbConnection dbc = new dbConnection();
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        try
//        {
//            string Orderid123 = "";
//            if (!IsPostBack)
//            {
//                //string data = Session["OrderId"].ToString();

//                //starting 20_10_2019
//                if (!string.IsNullOrWhiteSpace(Request.QueryString["Orderid"]))
//                {
//                    Orderid123 = Request.QueryString["Orderid"].ToString();
//                }

//                if (Session["OrderId"] != null && Session["OrderId"] != "")
//                {
//                    Orderid123 = Session["OrderId"].ToString();
//                }
//                string orderrdata = clsCommon.Base64Decode(Orderid123);
//                int oid = 0;
//                int.TryParse(orderrdata.ToString(), out oid);
//                if (oid > 0)
//                {

//                    //string addressstr = "select FirstName+' ' +LastName as CustName,Address,(select CityName from CityMaster where CityMaster.Id=CustomerAddress.CityId)as CityName,CustomerAddress.pincode,(select StateMaster.StateName from StateMaster where StateMaster.Id=CustomerAddress.StateId) as StateName,(select CountryMaster.CountryName from CountryMaster where CountryMaster.Id=CustomerAddress.CountryId)as CountryName,CustomerAddress.MobileNo from CustomerAddress where Id=(select AddressId from [Order] where id="+oid+") ;";

//                    string addressstr = "select FirstName as CustName,Address,(select CityName from CityMaster where CityMaster.Id=CustomerAddress.CityId)as CityName,CustomerAddress.pincode,(select StateMaster.StateName from StateMaster where StateMaster.Id=CustomerAddress.StateId) as StateName,(select CountryMaster.CountryName from CountryMaster where CountryMaster.Id=CustomerAddress.CountryId)as CountryName,CustomerAddress.MobileNo from CustomerAddress where Id=(select AddressId from [Order] where id=" + oid + ") ;";

//                    DataTable dtdataa = dbc.GetDataTable(addressstr);




//                    if (dtdataa != null && dtdataa.Rows.Count > 0)
//                    {
//                        lbladdname.InnerHtml = dtdataa.Rows[0]["CustName"].ToString();
//                        lbladd.InnerHtml = dtdataa.Rows[0]["Address"].ToString() + ", " + dtdataa.Rows[0]["CityName"] + "-" + dtdataa.Rows[0]["pincode"].ToString();
//                        lbladdstate.InnerHtml = dtdataa.Rows[0]["StateName"] + ", " + dtdataa.Rows[0]["CountryName"];
//                        lbladdmob.InnerHtml = dtdataa.Rows[0]["MobileNo"].ToString();
//                    }

//                    string OrderDetails = "select [Order].BuyWith,[Order].CustOfferCode,[Order].Id as oid,  (DATENAME(dw,CAST(DATEPART(m, CreatedOnUtc) AS VARCHAR)+ '/'+ CAST(DATEPART(d, CreatedOnUtc) AS VARCHAR)  + '/' + CAST(DATEPART(yy, CreatedOnUtc) AS VARCHAR))) +' '+convert(varchar(12),CreatedOnUtc,106)+', '+convert(varchar(12),CreatedOnUtc,108) as EndDate,ISNULL([Order].OrderMRP,0) as MRP,ISNULL([Order].OrderTotal,0)as OrderTotal,[Order].TotalQTY,isnull((select top 1 Name from Payment_Methods where [Order].PaymentGatewayId=Payment_Methods.Id),'Default') as GatwayType  from [Order] where [Order].Id=" + oid + "";

//                    DataTable dtorderdetails = dbc.GetDataTable(OrderDetails);

//                    string imaagedetails = "select Product.ProductMrp,Product.Mrp,Product.BuyWith1FriendExtraDiscount,Product.BuyWith5FriendExtraDiscount,Product.id as pid,product.Name,isnull(Unit,'0') as unitweg,isnull((select UnitName from UnitMaster where UnitMaster.Id=Product.UnitId),'Gram')as Unit from Product where Product.Id=(select ProductId from orderitem where orderid=" + oid + ")";
//                    DataTable dtimgstr = dbc.GetDataTable(imaagedetails);

//                    string productid = dtimgstr.Rows[0]["pid"].ToString();
//                    bool status = clsCommon.ProductStatus(productid);
//                    if (status == true)
//                    {
//                        disp.InnerHtml = "";                                        
//                    }
//                    if (dtorderdetails != null && dtorderdetails.Rows.Count > 0)
//                    {
//                        lblorderid.InnerHtml = dtorderdetails.Rows[0]["oid"].ToString();
//                        orderdatedid.InnerHtml = dtorderdetails.Rows[0]["EndDate"].ToString();
//                        lblmrp.InnerHtml = dtimgstr.Rows[0]["ProductMrp"].ToString();
//                        lbltotordeamt.InnerHtml = dtorderdetails.Rows[0]["OrderTotal"].ToString();
//                        lblqtyno.InnerHtml = dtorderdetails.Rows[0]["TotalQTY"].ToString();
//                        lblpayment.InnerHtml = dtorderdetails.Rows[0]["GatwayType"].ToString();

//                        string productidd = dtimgstr.Rows[0]["pid"].ToString();
//                        string flag = dtorderdetails.Rows[0]["BuyWith"].ToString();
//                        string custorder = dtorderdetails.Rows[0]["CustOfferCode"].ToString();

//                        string mess = "";
//                        string forcust = "";
//                        string mrp = "";
//                        mrp = dtimgstr.Rows[0]["Mrp"].ToString();
//                        string date = clsCommon.getProductExpiredOnDate(productidd);
//                        string ocode = "";
//                        if (custorder != null && custorder != "")
//                        {
//                            ocode = "/?offercode=" + custorder;
//                        }
//                        if (flag == "1")
//                        {
//                            mess = "Share this offer on WhatsApp so that your friends can also make the most if it!";
//                        }
//                        else if (flag == "2")
//                        {
//                            forcust = dtimgstr.Rows[0]["BuyWith1FriendExtraDiscount"].ToString();

//                            //string title = "Final Step";
//                            string[] daaata = forcust.Split('.');
//                            forcust = daaata[0].ToString();
//                            string[] mrpdata = mrp.Split('.');
//                            mrp = mrpdata[0].ToString();
//                            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
//                            mess = "Since you have chosen to buy with 1 friend, share offer to ensure your friend buys it by " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.Link sosho.in";


//                        }
//                        else if (flag == "6")
//                        {
//                            forcust = dtimgstr.Rows[0]["BuyWith5FriendExtraDiscount"].ToString();
//                            string[] daaata = forcust.Split('.');
//                            forcust = daaata[0].ToString();


//                            string[] mrpdata = mrp.Split('.');
//                            mrp = mrpdata[0].ToString();
//                            //string title = "Final Step";

//                            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
//                            mess = "Since you have chosen to buy with 4 friends, share offer to ensure your friends buy it by  " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.";
//                        }



//                        lblmsgtest.InnerHtml = "<span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"web\" href=\"https://web.whatsapp.com/send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + " \"share/whatsapp/share\" target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span> <span class=\"whatsapp\"><a class=\"fa fa-whatsapp\" id=\"mo-wa\" href=\"whatsapp://send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + " target=\"_blank\" style=\"cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;\"></a></span>";


//                    }




//                    if (dtimgstr.Rows.Count > 0)
//                    {
//                        lblnamee.InnerHtml = dtimgstr.Rows[0]["Name"].ToString();
//                        lblweigh.InnerHtml = dtimgstr.Rows[0]["unitweg"].ToString() + " " + dtimgstr.Rows[0]["Unit"];



//                        string productidd = dtimgstr.Rows[0]["pid"].ToString();
//                        string imagename = "select ImageFileName from ProductImages where Productid=" + productidd + "";
//                        DataTable dtimage = dbc.GetDataTable(imagename);
//                        string query = "SELECT [KeyValue] FROM [StringResources] where KeyName='ProductImageUrl'";
//                        DataTable dtfolder = dbc.GetDataTable(query);
//                        string folder = "";
//                        if (dtfolder.Rows.Count > 0)
//                        {
//                            folder = dtfolder.Rows[0]["KeyValue"].ToString();
//                        }
//                        string imgname = folder + dtimage.Rows[0]["ImageFileName"].ToString();

//                        lblimg123.InnerHtml = "<img src=" + imgname + ">";

//                    }


//                }
//            }

//        }
//        catch (Exception ee)
//        {
//        }
//    }



//}