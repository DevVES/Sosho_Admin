using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_WhatsappMsg : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        string qry = "SELECT [Id],[ImgURL],[Key],[Value] FROM [WhatsAppMsg]";
        DataTable dt = dbc.GetDataTable(qry);

        if (dt != null && dt.Rows.Count > 0)
        {
            HiddenField1.Value = dt.Rows[0]["Key"].ToString();
            TextBox1.Text = dt.Rows[0]["Value"].ToString();

            HiddenField2.Value = dt.Rows[1]["Key"].ToString();
            TextBox2.Text = dt.Rows[1]["Value"].ToString();

            HiddenField3.Value = dt.Rows[2]["Key"].ToString();
            TextBox3.Text = dt.Rows[2]["Value"].ToString();

            HiddenField4.Value = dt.Rows[3]["Key"].ToString();
            TextBox4.Text = dt.Rows[3]["Value"].ToString();

            HiddenField5.Value = dt.Rows[4]["Key"].ToString();
            TextBox5.Text = dt.Rows[4]["Value"].ToString();

            HiddenField6.Value = dt.Rows[5]["Key"].ToString();
            TextBox6.Text = dt.Rows[5]["Value"].ToString();
        }
    }
    [WebMethod]
    public static string HomepageMsg(string Msg, string Key)
    {
        dbConnection dbc = new dbConnection();
        try
        {
            string responce = "";
            string Message = Msg;
            string KeyVal = Key;
            if (Key != "" && Key != null)
            {
                string Update = "UPDATE [dbo].[WhatsAppMsg] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
                int res = dbc.ExecuteQuery(Update);
                if (res > 0)
                {
                    return responce = "Success";
                }
                else
                {
                    return responce = "Fail";
                }
            }
            return responce = "Fail";
        }
        catch (Exception)
        {
            return null;
        }
    }

    //[WebMethod]
    //public static string OrderSumMsg(string Msg, string Key)
    //{
    //    dbConnection dbc = new dbConnection();
    //    try
    //    {
    //        string responce = "";
    //        string Message = Msg;
    //        string KeyVal = Key;
    //        if (Key != "" && Key != null)
    //        {
    //            string Update = "UPDATE [dbo].[WhatsAppMsg] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
    //            int res = dbc.ExecuteQuery(Update);
    //            if (res > 0)
    //            {
    //                return responce = "Success";
    //            }
    //            else
    //            {
    //                return responce = "Fail";
    //            }
    //        }
    //        return responce = "Fail";
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}
    //[WebMethod]
    //public static string OrderDetmsg(string Msg, string Key)
    //{
    //    dbConnection dbc = new dbConnection();
    //    try
    //    {
    //        string responce = "";
    //        string Message = Msg;
    //        string KeyVal = Key;
    //        if (Key != "" && Key != null)
    //        {
    //            string Update = "UPDATE [dbo].[WhatsAppMsg] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
    //            int res = dbc.ExecuteQuery(Update);
    //            if (res > 0)
    //            {
    //                return responce = "Success";
    //            }
    //            else
    //            {
    //                return responce = "Fail";
    //            }
    //        }
    //        return responce = "Fail";
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}
    //[WebMethod]
    //public static string BuyAloneMsg(string Msg, string Key)
    //{
    //    dbConnection dbc = new dbConnection();
    //    try
    //    {
    //        string responce = "";
    //        string Message = Msg;
    //        string KeyVal = Key;
    //        if (Key != "" && Key != null)
    //        {
    //            string Update = "UPDATE [dbo].[WhatsAppMsg] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
    //            int res = dbc.ExecuteQuery(Update);
    //            if (res > 0)
    //            {
    //                return responce = "Success";
    //            }
    //            else
    //            {
    //                return responce = "Fail";
    //            }
    //        }
    //        return responce = "Fail";
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}
    //[WebMethod]
    //public static string ButwithOne(string Msg, string Key)
    //{
    //    dbConnection dbc = new dbConnection();
    //    try
    //    {
    //        string responce = "";
    //        string Message = Msg;
    //        string KeyVal = Key;
    //        if (Key != "" && Key != null)
    //        {
    //            string Update = "UPDATE [dbo].[WhatsAppMsg] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
    //            int res = dbc.ExecuteQuery(Update);
    //            if (res > 0)
    //            {
    //                return responce = "Success";
    //            }
    //            else
    //            {
    //                return responce = "Fail";
    //            }
    //        }
    //        return responce = "Fail";
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}
    //[WebMethod]
    //public static string BuywithFive(string Msg, string Key)
    //{
    //    try
    //    {

    //    }
    //    catch (Exception)
    //    { }
    //    return null;
    //}
}