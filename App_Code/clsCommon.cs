using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WebApplication1;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for clsCommon
/// </summary>
public class clsCommon
{
    //Api Url
    //Local
// public static string strApiUrl = "http://192.168.1.122";
    //Staging
    public static string strApiUrl = "http://api.salebhai.in:97";
    //Live
    //public static string strApiUrl = "http://api.salebhai.in";
    public static string rssymbol = "₹";

    public static string DeliveryTimeLine = "Delivery within India in 4-7 business days";
    public static string Pincodemaxlen6erromsg = "Enter Valid Pincode Number";

	public clsCommon()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public enum BannerActionType
    {
        OpenUrl = 1,
        NavigateToCategory = 2,
        AddToCart = 3,
        None = -1
    }

    //Webapicall fucntion
    public static string GET(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        try
        {
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            WebResponse errorResponse = ex.Response;
            using (Stream responseStream = errorResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                String errorText = reader.ReadToEnd();
                // log errorText
            }
            throw;
        }
    }

    public static string getProductExpiredOnDate(string prodid = "")
    {
        dbConnection dbc = new dbConnection();
        string where = "";
        if (prodid == "")
        {
            where = "where Product.StartDate<='" + dbc.getindiantime().ToString("dd-MMM-yyyy") + " 00:00:00' AND EndDate>='" + dbc.getindiantime().ToString("dd-MMM-yyyy") + " 23:59:59'";
        }
        else
        {
            where = "where Product.Id=" + prodid + "";
        }

        string date = "select CONVERT(varchar,EndDate,106) as dateprod, CONVERT(varchar,EndDate,100)as timeprod from Product " + where + " ";
        DataTable dt = dbc.GetDataTable(date);
        string edate = "";
        if (dt != null && dt.Rows.Count > 0)
        {
            edate = dt.Rows[0]["dateprod"].ToString();

            string timeeeee = "";

            string timeee = dt.Rows[0]["timeprod"].ToString();
            string[] time = timeee.Split(' ');
            int t1 = time.Length;
            timeeeee = time[t1 - 1];
            edate = edate + ' ' + timeeeee;
        }
        return edate;
    }
    public static bool ProductStatus(string productid = "")
    {
        dbConnection dbc = new dbConnection();
        string where = "";
        if (productid != "")
        {
            where = " and  Product.id=" + productid;
        }

        string expprostr = "select Product.Id as ProductId from Product where StartDate<='" + dbc.getindiantime().ToString("dd/MMM/yyyy HH:mm:ss tt") + "' AND EndDate>='" + dbc.getindiantime().ToString("dd/MMM/yyyy HH:mm:ss tt") + "' " + where;

        DataTable dtproduct = dbc.GetDataTable(expprostr);

        if (dtproduct.Rows.Count == 0)
        {
            return true;
        }
        return false;
    }

    public static string WhatsappmsgKey(string key)
    {


        //var request = (HttpWebRequest)WebRequest.Create("http://192.168.1.113:8089/api/MasterData/ReturnMessage?Key=" + key);  //Local
        var request = (HttpWebRequest)WebRequest.Create("http://api.salebhai.in/api/MasterData/ReturnMessage?Key=" + key);
        var response = (HttpWebResponse)request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //dbConnection dbc = new dbConnection();
        //string givekey = "select top 1 Id,[Value] from WhatsAppMsg where [Key] = '" + key + "'";
        //DataTable dtgetkey = dbc.GetDataTable(givekey);
        //if (dtgetkey != null && dtgetkey.Rows.Count > 0)
        //{
        //    string returnkey = dtgetkey.Rows[0]["Value"].ToString();
        //    return returnkey;
        //}
        JObject json = JObject.Parse(responseString);
        string message = json["Message"].ToString();

        return message;
    }
    public static string sweetMessage(string message, string time, string type,string position, string redirectOn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("callSweetAlertNotification('" + message + "', '" + time + "','" + type + "','" + position + "');");
            if (redirectOn != "")
            {
                sb.Append(" setTimeout(function () {window.location='" + redirectOn + "';}, " + time + ")");
            }
            sb.Append("</script>");
            return sb.ToString();
        }
        catch (Exception ee)
        {

            return "";
        }
    }
    public static currentCustomer getCurrentCustomer()
    {
        
        if (HttpContext.Current.Request.Cookies.AllKeys.Contains("TUser"))
        {
            currentCustomer objCurrentCustomer = new currentCustomer();
            objCurrentCustomer.id = HttpContext.Current.Request.Cookies["TUser"]["id"];
            objCurrentCustomer.name = HttpContext.Current.Request.Cookies["TUser"]["name"];
            objCurrentCustomer.mobile = HttpContext.Current.Request.Cookies["TUser"]["mobile"];
            objCurrentCustomer.email = HttpContext.Current.Request.Cookies["TUser"]["email"];
            return objCurrentCustomer;
        }
        return null;
    }
   

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    //coding encoding

    public static string Base64UrlEncode(byte[] input)
    {
        var output = Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", string.Empty);
        return output;
    }

    public static byte[] Base64UrlDecode(string input)
    {
        var output = input;
        // 62nd char of encoding
        output = output.Replace('-', '+');
        // 63rd char of encoding
        output = output.Replace('_', '/');
        // Pad with trailing '='s
        switch (output.Length % 4)
        {
            case 0:
                // No pad chars in this case
                break;
            case 2:
                // Two pad chars
                output += "=="; break;
            case 3:
                // One pad char
                output += "="; break;
            default:
                throw new InvalidOperationException("Illegal base64url string!");
        }

        // Standard base64 decoder
        return Convert.FromBase64String(output);
    }

    public static int GenerateRandomNumber()
    {
        Random rand = new Random((int)DateTime.Now.Ticks);
        //int numIterations = 0;
        //numIterations = rand.Next(1, 100);
        //Response.Write(numIterations.ToString());


        //Random rand = new Random();
        return rand.Next(100000, 999999);
        string PasswordLength = "6";
        string NewPassword = "";
        string allowedChars = "";
        allowedChars = "1,2,3,4,5,6,7,8,9";
        char[] sep = { ',' };
        string[] arr = allowedChars.Split(sep);
        string IDString = "";
        string temp = "";
        Random rand1 = new Random();
        for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
        {
            temp = arr[rand.Next(0, arr.Length)];
            IDString += temp;
            NewPassword = IDString;
        }
        int ans = Convert.ToInt32(NewPassword);
        return ans;
    }

    //store in varible and passs below function 

    public static object custdetails(string Custid)
    {
        dbConnection dbc = new dbConnection();
        try
        {
            string currentproduct = "select top 1 Product.id from Product where StartDate<=GETDATE()AND EndDate>=GETDATE() order by Product.id desc";

            DataTable dt = dbc.GetDataTable(currentproduct);
           
            if(dt!=null && dt.Rows.Count>0)
            {

                string productidd = dt.Rows[0]["Id"].ToString();
                string querydata = "select (select top 1 startdate from Product where Product.id=OrderItem.ProductId) as Productstarttime,(select top 1 startdate from Product where Product.id=OrderItem.ProductId) as Productendtime,OrderItem.Id as Orderitemid,OrderItem.ProductId as Productid,convert(varchar,[Order].CreatedOnUtc,108) as Ordertime ,convert(varchar,[Order].CreatedOnUtc,103) as Orderdate ,[Order].id as OrderId,[Order].BuyWith,ISNULL([Order].CustOfferCode,0) as CustOfferCode ,ISNULL([Order].RefferedOfferCode,0) as RefferedOfferCode  from [Order] inner join OrderItem on [Order].Id=OrderItem.OrderId where CustomerId=" + Custid + " and ProductId='" + productidd + "' and [Order].IsPaymentDone=1";

                DataTable dtorderdata = dbc.GetDataTable(querydata);

                if(dtorderdata!=null && dtorderdata.Rows.Count>0)
                {
                    clsModals.custorderdetails objcustdataa = new clsModals.custorderdetails();
                    {
                        objcustdataa.Response = "1";
                        objcustdataa.Message = "Successfully Done";
                        objcustdataa.Orderid = dtorderdata.Rows[0]["OrderId"].ToString();
                        objcustdataa.OrderItemId = dtorderdata.Rows[0]["Orderitemid"].ToString();
                        objcustdataa.ProductId = dtorderdata.Rows[0]["Productid"].ToString();
                        objcustdataa.Buywith = dtorderdata.Rows[0]["BuyWith"].ToString();
                        objcustdataa.OrderDate = dtorderdata.Rows[0]["Orderdate"].ToString();
                        objcustdataa.OrderTime = dtorderdata.Rows[0]["Ordertime"].ToString();
                        objcustdataa.Custoffercode = dtorderdata.Rows[0]["CustOfferCode"].ToString();
                        objcustdataa.ReferCode = dtorderdata.Rows[0]["RefferedOfferCode"].ToString();
                        objcustdataa.ProductEnddate = dtorderdata.Rows[0]["Productendtime"].ToString();
                        objcustdataa.ProductStartDate = dtorderdata.Rows[0]["Productstarttime"].ToString();
                        return objcustdataa;
                    }
                }
                else
                {
                    clsModals.custorderdetails objcust1= new clsModals.custorderdetails();
                    objcust1.Response = "0";
                    objcust1.Message = "No Data Found";
                }
            }

            clsModals.custorderdetails objcust = new clsModals.custorderdetails();
            objcust.Response = "0";
            objcust.Message = "No Data Found";
          

            
        }
        catch (Exception ee)
        {

           
        }
        return "";
    }

   
}
