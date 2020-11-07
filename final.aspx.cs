using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using WebApplication1;

public partial class final : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                //testing 
                //Session["PlaceOrderId"] = 46;
                string Porderid = "0";
                string BuyFlag = "";
                string ccode = "";

                Session["ReferCode"] = "";

                if ((HttpContext.Current.Session["CouponCode"] != null) && (HttpContext.Current.Session["CouponCode"] != ""))
                {
                    ccode = Session["CouponCode"].ToString();
                    //ccode = clsCommon.Base64Decode(couponpcode);
                    // lblccode.Text = refcode;
                }
                string buyflagen = "";
                if ((HttpContext.Current.Session["BuyFlag"] != null) && (HttpContext.Current.Session["BuyFlag"] != ""))
                {
                    buyflagen = Session["BuyFlag"].ToString();
                    BuyFlag = clsCommon.Base64Decode(buyflagen);

                }

                string data = "";
                if (Session["PlaceOrderId"] != null)
                {
                    Porderid = Session["PlaceOrderId"].ToString();
                    data = clsCommon.Base64Decode(Porderid);
                    lblorderid.InnerHtml = data;
                    Porderid = data;
                    lbldeliveryline.InnerHtml = clsCommon.DeliveryTimeLine;
                    int orderId = 0;
                    int.TryParse(Porderid.ToString(), out orderId);
                    if (orderId > 0)
                    {
                        string Querypaymenttype = "select top 1 Id,Name from payment_methods where id=(select PaymentGatewayId from [Order] where [Order].Id=" + orderId + ")";
                        DataTable dtpayment = dbc.GetDataTable(Querypaymenttype);

                        if (dtpayment.Rows.Count > 0)
                        {
                            paymentype.InnerHtml = dtpayment.Rows[0]["Name"].ToString();
                        }
                        string Custid = clsCommon.getCurrentCustomer().id;

                        if (Custid != null && Custid != "")
                        {
                            clsModals.custorderdetails objcustdetails = clsCommon.custdetails(Custid);

                            string productidd = objcustdetails.ProductId;
                            DataTable dtproduct = dbc.GetDataTable("select * from Product where id=" + productidd + "");
                            if (dtproduct != null && dtproduct.Rows.Count > 0)
                            {
                                string proname = dtproduct.Rows[0]["Name"].ToString();
                                Span1.InnerHtml = proname;

                                DataTable custaddr = dbc.GetDataTable("select FirstName,Address,PinCode,(select Cityname from CityMaster where CityMaster.Id=CustomerAddress.CityId) as Cityname from CustomerAddress where id=(select AddressId from [Order] where [Order].Id=" + orderId + ")");

                                if (custaddr != null && custaddr.Rows.Count > 0)
                                {
                                    lbladdridd.InnerHtml = custaddr.Rows[0]["FirstName"] + " " + custaddr.Rows[0]["Address"] + "-" + custaddr.Rows[0]["PinCode"];
                                }

                                DataTable custorderqty = dbc.GetDataTable("select TotalQTY from [Order] where [Order].Id=" + orderId + "");
                                if (custorderqty != null && custorderqty.Rows.Count > 0)
                                {
                                    string qtyy = custorderqty.Rows[0]["TotalQTY"].ToString();
                                    string[] data123 = qtyy.Split('.');
                                    Span2.InnerHtml = data123[0];
                                }
                            }
                        }
                    }
                }
                else
                {
                    orderdone.InnerText = "";
                    ordernotdon.Visible = true;
                    //ltrerr.Text = "Order Details Not Found ";
                    ltrerr.Text = "Order Details Not Found ";
                }

                string qry = "select Product.Id as ProductId, Product.[DisplayNameInMsg],Product.[ShowMrpInMsg],  Product.Mrp, Product.BuyWith1FriendExtraDiscount, Product.BuyWith5FriendExtraDiscount, (CONVERT(varchar ,Product.EndDate,106)) as pedate , (CONVERT(varchar ,Product.EndDate,100)) as pedate1, [Order].Id as orderid, OrderItem.Id as orderitemid,Product.Name,Convert(int,Product.ProductMrp) as ProductMrp from Product Inner join OrderItem on OrderItem.ProductId=Product.Id Inner Join [Order] On [Order].Id = OrderItem.OrderId  where [Order].Id=" + data;

                //
                DataTable dt = dbc.GetDataTable(qry);
                string ProductNameinmsg = "";
                bool ismrp = false;
                string mrp = "";
                string forcust = "";
                string date = "";
                string whatsapp = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    string flg = clsCommon.Base64Decode(buyflagen);
                    ProductNameinmsg = dt.Rows[0]["DisplayNameInMsg"].ToString();
                    if (ProductNameinmsg.Length == 0 || ProductNameinmsg == null || ProductNameinmsg == "")
                    {
                        ProductNameinmsg = dt.Rows[0]["Name"].ToString();
                    }




                    ismrp = bool.Parse(dt.Rows[0]["ShowMrpInMsg"].ToString());
                    mrp = dt.Rows[0]["Mrp"].ToString();
                    date = dt.Rows[0]["pedate"].ToString();
                    string date1 = dt.Rows[0]["pedate1"].ToString();
                    string[] dataaa = date1.Split(' ');
                    int lendata = dataaa.Length;

                    string time = dataaa[lendata - 1];

                    date = date + ' ' + time;

                    string mess = "";



                    if (flg == "1")
                    {
                        forcust = dt.Rows[0]["Mrp"].ToString();
                        string[] daaata = forcust.Split('.');
                        forcust = daaata[0].ToString();
                        mess = "Share this offer on WhatsApp so that your friends can also make the most if it!";
                        whatsapp = clsCommon.WhatsappmsgKey("BuyAlone");
                    }
                    else if (flg == "2")
                    {
                        forcust = dt.Rows[0]["BuyWith1FriendExtraDiscount"].ToString();
                        //string title = "Final Step";
                        string[] daaata = forcust.Split('.');
                        forcust = daaata[0].ToString();
                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
                        mess = "Since you have chosen to buy with 1 friend, share offer to ensure your friend buys it by " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.";
                        whatsapp = clsCommon.WhatsappmsgKey("BuyWithOneFrd");
                    }
                    else if (flg == "6")
                    {
                        forcust = dt.Rows[0]["BuyWith5FriendExtraDiscount"].ToString();
                        string[] daaata = forcust.Split('.');
                        forcust = daaata[0].ToString();
                        //string title = "Final Step";

                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
                        mess = "Since you have chosen to buy with 4 friends, share offer to ensure your friends buy it by  " + date + " to pay offer price of only ₹" + forcust + " instead of single-buy price of ₹" + mrp + " at time of delivery.";
                        whatsapp = clsCommon.WhatsappmsgKey("ButWith4Frd");
                    }
                    
                    lblmsg.InnerHtml = mess;

                }



                if (ccode != "" && BuyFlag != "")
                {


                    int Mrpint = 0;
                    int.TryParse(mrp.ToString(), out Mrpint);
                    int forcustint = 0;
                    int.TryParse(forcust.ToString(), out forcustint);

                    string[] price = mrp.Split('.');
                    string[] cust = forcust.Split('.');

                    mrp = price[0].ToString();
                    forcust = cust[0].ToString();


                    string timeee = clsCommon.getProductExpiredOnDate();
                    string[] time = timeee.Split(' ');
                    int t1 = time.Length;
                    string timeeeee = time[t1 - 1];
                    string mrpd = "";
                    if (ismrp == true)
                    {
                        mrpd = "(MRP ₹ " + dt.Rows[0]["ProductMrp"].ToString() + ")";
                    }

                    // date = date + " " + timeeeee;
                    if (ccode == "0")
                    {
                        ccode = "";
                    }


                    string ocode = "";
                    if (ccode != "0" && ccode != "")
                    {
                        ocode = "/?offercode=" + ccode;
                    }

                    string msg = "Hi! I just bought " + ProductNameinmsg + " at just ₹" + forcust + " " + mrpd + ". Free shipping and Cash on delivery. If you buy it before " + date + ", you can also get the same discount. Your Offer Code Is " + ccode + "";
                    Label1.Text = msg;

                    lblwhatsapp.InnerHtml = "whatsapp://send?text=Hi! I just bought " + ProductNameinmsg + " at just ₹" + forcust + " " + mrpd + ". Free shipping and Cash on delivery. If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in";

                    //lblwhatsapp.InnerHtml = "<div class=\"order-offer-code\"> <p class=\"\" style=\"display:inline-flex\"> SHARE OFFER ON : <a id=\"webscreen\" target=\"_blank\" href=\"https://web.whatsapp.com/send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + "" + ccode + " \"share/whatsapp/share\"> <img id=\"web\" src=\"images/whatsapp.png\" /></a>   <a id=\"mobscreen\"  target=\"_blank\" href='whatsapp://send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in" + ocode + "'> <img id=\"mo-wa\" src=\"images/whatsapp.png\" /></a> </p>  </div>";

                    lblwhatsapp.InnerHtml = " <a id=\"webscreen\" class=\"wa-btn\" target=\"_blank\" href=\"https://web.whatsapp.com/send?text=" + whatsapp + "" + ocode + " \"share/whatsapp/share\"> Share on whatsapp <i class=\"fa fa-whatsapp\"></i></a>  <a id=\"mobscreen\" class=\"wa-btn\"  target=\"_blank\" href='whatsapp://send?text=" + whatsapp + "" + ocode + "'>Share on whatsapp <i class=\"fa fa-whatsapp\"></i></a>";

                    //whatsapp2.InnerHtml = "<div class=\"order-offer-code\"> <p class=\"\" style=\"display:inline-flex\"> SHARE OFFER ON : <a target=\"_blank\" href=\"https://web.whatsapp.com/send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in/?offercode=" + ccode + " \"share/whatsapp/share\"> <img id=\"web\" src=\"images/whatsapp.png\" /></a>   <a target=\"_blank\" href='whatsapp://send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in/?offercode=" + ccode + "'> <img id=\"mo-wa\" src=\"images/whatsapp.png\" /></a> </p>  </div>";

                    //whatsapp2.InnerHtml = "<div class=\"order-offer-code\"> <p class=\"\" style=\"display:inline-flex\"> SHARE OFFER ON : <a  target=\"_blank\" href=\"https://web.whatsapp.com/send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in/?offercode=" + ccode + " \"share/whatsapp/share\"> <img id=\"web\" src=\"images/whatsapp.png\" /></a>   <a target=\"_blank\" href='whatsapp://send?text=Hi! I just bought this awesome product at just ₹" + forcust + ". If you buy it before " + date + ", you can also get the same discount. Just follow this link: http://www.sosho.in/?offercode=" + ccode + "'> <img id=\"mo-wa\" src=\"images/whatsapp.png\" /></a> </p>  </div>";


                }
                else
                {
                    lblwhatsapp.InnerHtml = "";
                }
            }
        }
        catch (Exception ee)
        {
            ltrerr.Text = ee.StackTrace;
        }
    }

    public string getcouponcode()
    {
        try
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }
        catch (Exception ee)
        {
            return "";
        }
    }
}